# XMLStats
Generates a report on existing XML files. Useful if you have many XML files to study.
Useful for composite apps or BPM files.

You can focus on some elements or attributes to drilldown.

Running the code as is will return a list of elements and attributes

For each elment there is a summary sample:
```
<service namespace="3" name="3" wsdlLocation="3"/>
```
This summary states that the "service" tag appears with 3 attributes - namespace, name and wsdlLocation. The number represents the number of times that this attribute appears.


```
FILES : 30
ERRORS: 2
NODES AND COUNT + ATTRIBUTES

serviceMetadata 2
<serviceMetadata/>
   Descendants
      service   3

service 6
<service namespace="3" name="3" wsdlLocation="3"/>
   Descendants
      description       3
      interface.wsdl    3
      binding.ws        3
   Attributes
      name      3
      namespace 3
      wsdlLocation      3

description     4
<description/>
   Descendants
      #text     4

#text   3143
<#text/>

assets  2
<assets/>
   Descendants
      asset     7

asset   7
<asset alias="7" local="7" sourceType="7" type="7" charset="4" recno="4"/>
   Descendants
      location  7
   Attributes
      alias     7
      charset   4
      local     7
      recno     4
      sourceType        7
      type      7
...
```