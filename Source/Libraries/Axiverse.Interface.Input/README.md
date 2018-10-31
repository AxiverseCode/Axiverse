Unity uses action maps.

We want to have an action set.
We want to have default mappings based on input devices and types.
- These can have semantic meaning intrinsically or applied via config.
We want

Currently we only care about handling single player.


Classes
- ControlSet - a group of controls with default semantic mapping
- SemanticMapping - mapping between a specific device and different semantics

- Events include value (state changes). Control sets themselves can retain values.

- Keyboard and mouse handled separately?
	- Acquire exclusive access ownership over cursor/keyboard? (PriorityRouting)
	- How to handle global state?