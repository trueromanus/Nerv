# Nerv
Ultra light reactive library for XAML-binding applications for NETStandart 2.0.

## Features

- Source code writing simple as much as possible and will be kept small size in the future.
- If you want to avoid writing boilerplate code without postprocessors or complex libraries that being a black box.
- Use library doesn't require ViewModel class will be inherited from classes or decorated attributes.
- Zero dependencies. Only NETStandart 2.0.
- Support with all XAML frameworks

## Documentation

[Main documentation](https://github.com/trueromanus/Nerv/wiki/Documentation)

## Hello world example

```csharp
public class ViewModel {

  public ReactiveProperty<string> CoolProperty { get; private set; }
    
  public ParameterlessCommand ChangeValue { get; private set; }

  public MainPageViewModel () {
    CoolProperty = new ReactiveProperty<string> ( "" );
    ChangeValue = new ParameterlessCommand( changeValue );
  }
    
  public changeValue() {
    CoolProperty.SetValue( "Changed!!!!" );
  }    
}
```

```xml
<StackPanel>
  <TextBlock
    Text="{Binding CoolProperty.Value}"
  />
  <Button Command="{Binding ChangeValue}">
    Change
  </Button>
</StackPanel>
```
