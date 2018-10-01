# SXA-13922

[SXA] Rendering Variant renders unnecessary DIV when Container is marked as Is Link and Tag is empty

**1.** Create variant for component as follows

- `VariantDefinition`
  - `VariantReference` that iterates through Items field
    - `VariantSection` with `Tag = ""` and `Is link = "1"`
      - `VariantField` with `Tag = ""` and `Field name = "Title"`
      - `VariantField` with `Tag = ""` and `Field name = "Text"`

**2.** Render the variant against item with Items field that has two items selected
**3.** Check output HTML

**Actual Result:**

```html
<div>
  <a href="/item1" >
    <div>
      <h2>Item1 Title Field Value</h2>
      <span>Item1 Text Field Value</h2>
    </div>
  </a>
  <a href="/item2" >
    <div>
      <h2>Item2 Title Field Value</h2>
      <span>Item2 Text Field Value</h2>
    </div>
  </a>
</div>
```

**Expected Result:**

```html
<div>
  <a href="/item1" >
    <h2>Item1 Title Field Value</h2>
    <span>Item1 Text Field Value</h2>
  </a>
  <a href="/item2" >
    <h2>Item2 Title Field Value</h2>
    <span>Item2 Text Field Value</h2>
  </a>
</div>
```
