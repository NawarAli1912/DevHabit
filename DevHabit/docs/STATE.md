# STATE — the head of the session chain, read this first

Rewritten by `/session-end` from the latest session node + the previous head.
Only this file is injected into new sessions; per-session history lives in
`docs/sessions/`. Keep it short — detail belongs in DECISIONS.md and
ACHIEVEMENTS.md, which are append-only and never re-summarized.

## Project in one line

DevHabit — a habit-tracking REST API for developers (ASP.NET Core 10), run as a real product with a ClickUp backlog, built while working through Milan Jovanović's Pragmatic REST APIs course.

## Status (2026-07-07 — initialized, no session nodes yet)

**For newcomers:** DevHabit is a Web API for tracking developer habits, and it doubles as the owner's working ground for the Pragmatic REST APIs course — every course chapter becomes a shippable, PR-sized ticket on a ClickUp board. The habits endpoints already support validation, global exception handling, pagination, sorting, filtering, data shaping, and HATEOAS links. Right now the work in progress is content negotiation: letting clients ask for different response formats (custom media types) on the habits endpoints. That work sits uncommitted on `main`.

**Expert delta:**
- Committed through `a6f1384` (HATEOAS links on habit endpoints).
- Uncommitted WIP: content negotiation via `CustomMediaTypeNames` — touches `HabitsController`, `HabitDto`, `HabitsQueryParameters`, `DependencyInjection`, `DataShapingService`.
- Single-project solution (`DevHabit.Api`), warnings-as-errors + SonarAnalyzer solution-wide.
- ClickUp board (folder `901211718633`) is the source of truth for ticket status; epics track progress in their descriptions.

## Next

1. Finish and commit the in-progress custom media type / content negotiation work on the habits endpoints, then update the matching ClickUp ticket (`to do` → `in progress` → `complete`) and tick the parent epic checklist.

## Open questions

- None right now.
