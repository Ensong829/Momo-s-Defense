# Working Rules

## Development Rhythm

1. Pick one small feature.
2. Implement it.
3. Test it.
4. Fix blocking issues.
5. Update docs.
6. Move to the next feature.

## Scope Control

- Build the smallest playable version first.
- Placeholder assets are acceptable until gameplay proves itself.
- New systems must serve the current milestone.
- Upgrade systems are core, but they should arrive in layers.

## Testing Rule

Every implementation chunk should answer:

- What changed?
- How was it tested?
- What still feels risky?
- What is the next smallest step?

Before moving to the next feature:

- Check Unity compilation.
- Check Unity Console or `Editor.log` for current errors.
- Fix compile errors immediately.
- Fix blocking runtime errors immediately.
- Do not build new features on top of a known broken state.

The user should not need to send screenshots for routine errors. Prefer reading Unity logs directly whenever possible.

## Agent Rule

Sub-agents can be used for bounded tasks once the project has enough surface area.

The main agent remains responsible for:

- Planning.
- Integration.
- Reviewing changes.
- Keeping the docs current.
- Making sure the test/fix loop happens.
