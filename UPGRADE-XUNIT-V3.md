# Upgrade xUnit v2 → v3

Guida alla migrazione di F2.Testing da xUnit 2.9.x a xUnit v3.

Riferimento ufficiale: https://xunit.net/docs/getting-started/v3/migration

---

## 1. F2.Testing.csproj (libreria)

```diff
- <PackageReference Include="xunit.extensibility.core" Version="2.9.3" />
+ <PackageReference Include="xunit.v3.extensibility" Version="x.x.x" />
```

> Verificare il nome esatto del pacchetto su NuGet. Il mapping ufficiale è:
> `xunit.extensibility.core` + `xunit.extensibility.execution` → `xunit.v3.extensibility`

---

## 2. ServerFixture.cs

### Namespace

```diff
  using Xunit;
+ // IAsyncLifetime ora eredita IAsyncDisposable
```

### IAsyncLifetime — return type cambiato da `Task` a `ValueTask`

```diff
- public virtual Task InitializeAsync()
+ public virtual ValueTask InitializeAsync()
  {
      _client = CreateClient();
      _scope = Services.CreateScope();
-     return Task.CompletedTask;
+     return ValueTask.CompletedTask;
  }
```

### DisposeAsync — non serve più l'implementazione esplicita

In v3, `IAsyncLifetime` eredita `IAsyncDisposable`. `WebApplicationFactory` implementa già `IAsyncDisposable.DisposeAsync()`, quindi l'implementazione esplicita è ridondante.

```diff
- /// <summary>
- /// IAsyncLifetime necessario per inizializzare InitializeAsync
- /// </summary>
- /// <returns></returns>
- Task IAsyncLifetime.DisposeAsync()
- {
-     return base.DisposeAsync().AsTask();
- }
```

---

## 3. PriorityOrderer.cs

Cambiamenti principali:
- `ITestCaseOrderer` si sposta in `Xunit.v3`
- Firma cambia: `IEnumerable<TTestCase>` → `IReadOnlyCollection<TTestCase>` (input e output)
- Constraint: `where TTestCase : notnull, ITestCase`
- **Reflection abstractions rimosse**: `IAttributeInfo`, `IMethodInfo` non esistono più. Si usa `System.Reflection` diretto tramite cast a `IXunitTestCase`
- `testCase.TestMethod.Method.Name` → `testCase.TestMethodName`

### Prima (v2)

```csharp
using Xunit.Abstractions;
using Xunit.Sdk;

namespace F2.Testing.Ordering;

public class PriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (TTestCase testCase in testCases)
        {
            int priority = 0;

            foreach (IAttributeInfo attr in testCase.TestMethod.Method.GetCustomAttributes((typeof(TestPriorityAttribute).AssemblyQualifiedName)))
                priority = attr.GetNamedArgument<int>("Priority");

            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
        {
            list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
            foreach (TTestCase testCase in list)
                yield return testCase;
        }
    }

    // ...
}
```

### Dopo (v3)

```csharp
using System.Reflection;
using Xunit.Sdk;
using Xunit.v3;

namespace F2.Testing.Ordering;

public class PriorityOrderer : ITestCaseOrderer
{
    public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
        where TTestCase : notnull, ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (var testCase in testCases)
        {
            int priority = 0;

            if (testCase is IXunitTestCase xunitTestCase)
            {
                var attr = xunitTestCase.TestMethod.Method.GetCustomAttribute<TestPriorityAttribute>();
                if (attr != null)
                    priority = attr.Priority;
            }

            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        var result = new List<TTestCase>();
        foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
        {
            list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethodName, y.TestMethodName));
            result.AddRange(list);
        }
        return result;
    }

    static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));

        if (dictionary.TryGetValue(key, out TValue result)) return result;

        result = new TValue();
        dictionary[key] = result;

        return result;
    }
}
```

> **Nota:** Verificare che `IXunitTestCase.TestMethod.Method` sia il path corretto per accedere al `MethodInfo` in v3. Se il compilatore segnala errori, controllare la documentazione: https://xunit.net/docs/getting-started/v3/migration-extensibility

---

## 4. TestPriorityAttribute.cs

Nessuna modifica necessaria.

---

## 5. SerializerExtensions.cs

Nessuna modifica necessaria.

---

## 6. Riepilogo pacchetti

| Progetto | v2 | v3 |
|----------|----|----|
| F2.Testing | `xunit.extensibility.core` 2.9.3 | `xunit.v3.extensibility` |

---

## Checklist

- [ ] Aggiornare pacchetto NuGet in F2.Testing.csproj
- [ ] Aggiornare `ServerFixture.cs` (ValueTask + rimuovere DisposeAsync esplicito)
- [ ] Riscrivere `PriorityOrderer.cs` (nuova firma + reflection diretta)
- [ ] Compilare e verificare
- [ ] Aggiornare la versione del pacchetto NuGet
- [ ] Taggare il commit
- [ ] Applicare le modifiche ai progetti consumer (vedi patch `2026-03-11-xunit-v3-migration.md`)
