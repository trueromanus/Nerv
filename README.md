# Nerv
Ultra light reactive library for XAML-binding applications.

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
