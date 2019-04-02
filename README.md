# NikoSDK

<a target="_blank" href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License" /></a>
<a target="_blank" href="https://twitter.com/guruumeditation"><img src="https://img.shields.io/twitter/follow/guruumeditation.svg?style=social" /></a>

Niko Home Control SDK for .NET Standard

## Status

| Target | Branch | Status | SonarCloud | Package version |
|--------------|------------- | --------- | --------| --------|
| Release | master | <a target="_blank" href="https://equinoxe.visualstudio.com/Niko%20SDK/_build?latest?definitionId=34"><img src="https://equinoxe.visualstudio.com/Niko%20SDK/_apis/build/status/Pipeline%20Master%20Publish?branchName=master" alt="Build status" /></a> | <a target="_blank" href="https://sonarcloud.io/dashboard?id=Guruumeditation_NikoSDK"><img src="https://sonarcloud.io/api/project_badges/measure?project=Guruumeditation_NikoSDK&metric=alert_status" alt="Nuget package" /></a> | <a target="_blank" href="https://www.nuget.org/packages/Net.ArcanaStudio.NikoSDK/"><img src="https://img.shields.io/nuget/v/Net.ArcanaStudio.NikoSDK.svg" alt="Nuget package" /></a> |
| Preview | develop | <a target="_blank" href="https://equinoxe.visualstudio.com/Niko%20SDK/_build?latest?definitionId=33"><img src="https://equinoxe.visualstudio.com/Niko%20SDK/_apis/build/status/Pipeline%20Develop%20Publish?branchName=develop" alt="Build status" /></a>| <a target="_blank" href="https://sonarcloud.io/dashboard?id=Guruumeditation_NikoSDK"><img src="https://sonarcloud.io/api/project_badges/measure?project=Guruumeditation_NikoSDK&metric=alert_status" alt="Nuget package" /></a> | <a target="_blank" href="https://www.nuget.org/packages/Net.ArcanaStudio.NikoSDK/"><img src="https://img.shields.io/nuget/vpre/Net.ArcanaStudio.NikoSDK.svg" alt="Nuget package"/></a> |



## Usage
### Client instantiation

You can use the autodetect static method :
```
var client = NikoClient.AutoDetect();
```

Or pass the IP of the NHC :
```
var client = new NikoClient("168.192.1.11");
```
### Start client


To start the client, use :

```
client.StartClient();
```

### Stop client


To stop the client, use :

```
client.StopClient();
```


### Locations


To get the locations, use :

```
var locations = await client.GetLocations();
```

### Actions


To get the actions, use :

```
var actions = await client.GetActions();
```

### Execute Command


To execute an action, use :

```
var result = await client.ExecuteCommand(int id, int value);
```                       

Value is from 0 to 100, and you can get action id from the action list

### Events


To receive events like status change:

```
await client.StartEvents();
client.OnValueChanged += _nikoClient_OnValueChanged;

private void _nikoClient_OnValueChanged(object sender, IEvent e)
{
    ...
}
```

## Models

If you want to use the model for a client application, there is this nuget package :

<a target="_blank" href="https://www.nuget.org/packages/Net.ArcanaStudio.NikoSDK.Model/">Net.ArcanaStudio.NikoSDK.Model</a>


## Limitations

It wasn't tested on NHC II

As I don't have alarm or energy modules, there is not command for it.
If someone can give the their json format, I will implement it.

## History

1.0.3
- FIXED : Values sometimes not well returned from server
- CHANGE : Action's Location property renamed to LocationId

1.0.2
- FIXED : Case when multiple messages received same time

1.0.1:
- FIXED : bug when sometimes NHC send extra characters at the end of json

1.0
- Initial release

## License

```
Copyright 2019 Arcana Studio

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```
