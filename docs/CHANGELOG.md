### **v3.1.0.6767** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 16-02-2026
* [a1d829c] (RzR) -> Add general search in all fields `SearchInAllFields`.
* [ed3b656] (RzR) -> Add new script for version gen.
 
 
### **v3.0.0.0**
#### Breaking changes
-> Changed definition for `PredefinedRecords`!<br />
-> Rename from `PredefinedRecords` -> `PredefinedRecord`;<br />
-> Set `PredefinedRecord` as object with 2 properties: <br />
    - `PredefinedFieldName` - name of the field/column to identify records <br />
    - `PredefinedRecords` - identifiers of the predefined records<br />

FROM
```csharp
public class PagedRequest
{
    // ...
    public ICollection<string> PredefinedRecords { get; set; } = new     HashSet<string>();
    // ...
}
```

TO
```csharp
public class PagedRequest
{
    // ...
    public DataPredefinedFilterDefinition PredefinedRecord { get; set; } = new DataPredefinedFilterDefinition();
    // ...
}
```

```csharp
public class DataPredefinedFilterDefinition
{
    public string PredefinedFieldName { get; set; }
    
    public ICollection<string> PredefinedRecords { get; set; } = new     HashSet<string>();
}
```

### **v2.0.0.0**
-> Add new project with required data models; <br />
-> Update reference packages version; <br />
-> Fix the references and update related projects; <br />
-> Adjust XML/SOAP in paged result; <br />
-> Generate new version and update changelog file;<br />
-> Update readme file;<br />

### **v1.0.3.6458**
-> Update reference package version, fixing CVE (`CVE-2024-43485`);<br />

### **v1.0.1.1104**
-> Update libs; <br />
-> Add cast result to Xml/Soap result; <br />

### **v1.0.2.7174**
-> Update libs version; <br />
-> Fix using reference; <br />



