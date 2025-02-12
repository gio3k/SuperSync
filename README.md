# SuperSync for Unity Netcode

SuperSync is a tool that makes coding in Unity Netcode a bit easier.

### Sync Attributes

Create network variables easily using `[Sync]`

```cs
[Sync] public int MyVariable {get; set;}
[Sync] public bool MyOtherVariable {get; set;} = true;
```

### Networking Network Behaviours

All classes deriving from NetworkBehaviour support being arguments in RPCs.

```cs
public class Fish : NetworkBehaviour {
    // NetworkBehaviour
    // Component of ocean dwellers 
}

[Rpc(SendTo.Everyone)]
public void MyTestRpc(Fish fish) {
    
}

[Sync] public Fish MyFish {get; set;}
```

## Advanced Usage

### Anticipated Syncing

Anticipated variables can be set at any time, regardless of owner. If the owner sends the real value, it will be used.

For an anticipated variable, add the `[AlwaysWritable]` attribute to a property with the `[Sync]` attribute.

> [!IMPORTANT]  
> Anticipated variables don't support SyncFlags. This seems to be a limitation with Unity's AnticipatedNetworkVariable.


```cs
[Sync, AlwaysWritable] public int MyAnticipatedVariable {get; set;}
```

### Getting NetworkVariables

You may need the internal NetworkVariable at times. You can use SyncUtils for that:

```cs
[Sync] public int MyVariable {get; set;}
[Sync, AlwaysWritable] public int MyAnticipatedVariable {get; set;}

public void OnStart() {
    var networkVariable = SyncUtils.GetVariable(MyVariable);
    var anticipatedNetworkVariable = SyncUtils.GetAnticipatedVariable(MyAnticipatedVariable);
}
```

## License

SuperSync uses a modified MIT license that in simple terms says you can't distribute it on the Asset Store without
permission.

For more info, check LICENSE