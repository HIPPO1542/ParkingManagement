---
name: idiomatic-clean-code
description: >-
  Write code that is idiomatic, readable, maintainable, and easy for humans to understand. Apply by default to all coding tasks, including writing, modifying, reviewing, and refactoring code.
---

# Idiomatic, Clean Code

Write code that is idiomatic, readable, maintainable, and easy for humans to understand.

**Core principle:** Optimize for the next developer reading the code, not for the fewest lines written.

## 1. Usage and Priority

- Apply this skill by default to all coding tasks, including writing, modifying, reviewing, and refactoring code.
- This skill may be combined with other coding skills, such as language-specific, framework-specific, or architectural skills (e.g., DDD, CQRS).
- Respect functional requirements, architecture, APIs, and implementation constraints established by other applicable skills.
- When stylistic preferences conflict, prioritize readability, clarity, and maintainability.
- Explicit user instructions and project requirements take precedence over this skill.
- Do not sacrifice clarity merely to satisfy preferences for compactness, cleverness, abstraction, or stylistic novelty.

The examples below illustrate principles, not mandatory implementation templates. Adapt them to the language, framework, and codebase.

## 2. Naming

- Use descriptive, meaningful names for variables, functions, classes, methods, and modules.
- Names should communicate intent, purpose, or domain meaning rather than merely describing implementation details.
- Prefer longer names when they eliminate ambiguity.
- Avoid generic names such as `data`, `item`, `obj`, `tmp`, `x`, or `result` when more specific alternatives improve clarity.
- Short names are acceptable when their meaning is obvious from context, such as conventional loop indices or mathematical notation.
- Use consistent terminology throughout the codebase.
- Follow established naming conventions of the language, framework, and project.

### Example: Descriptive names

**Bad:**

```python
def calc(x, y):
    z = x - y
    return z
```

**Good:**

```python
def calculate_water_level_change(
    future_water_level: float,
    current_water_level: float,
) -> float:
    return future_water_level - current_water_level
```

The improved version communicates what the values represent and what the function computes.

An example of a longer name that eliminates ambiguity:

```python
total_number_of_successfully_processed_records = 100
```

**Rather than:**

```python
processed_record_count = 100
```

**Goal:** A reader should understand what something represents or does without needing to trace its implementation.

## 3. Readability and Control Flow

- Prefer straightforward, intuitive implementations over clever or unnecessarily compact alternatives.
- Keep code concise when conciseness improves or preserves readability.
- Do not compress multiple distinct operations into a single expression when doing so obscures their meaning.
- Prefer explicit control flow when it makes the logic easier to follow.
- Introduce intermediate variables when they clarify calculations, conditions, transformations, or business logic.
- Avoid unnecessary intermediate variables that merely repeat obvious information.
- Reduce deeply nested control flow when practical, using guard clauses, early returns, or appropriately extracted functions.
- Keep related logic together and make execution flow easy to follow.

### Example: Complex conditions

**Bad:**

```python
if user.age >= 18 and user.is_active and not user.is_banned and user.email_verified:
    grant_access(user)
```

**Good:**

```python
is_eligible_age = user.age >= 18
has_valid_account = user.is_active and not user.is_banned
has_verified_email = user.email_verified

if is_eligible_age and has_valid_account and has_verified_email:
    grant_access(user)
```

The intermediate variables communicate the meaning of each condition.

### Example: Avoid deep nesting

**Bad:**

```python
def process_order(order):
    if order is not None:
        if order.is_valid:
            if order.items:
                return submit_order(order)

    return None
```

**Good:**

```python
def process_order(order):
    if order is None:
        return None

    if not order.is_valid:
        return None

    if not order.items:
        return None

    return submit_order(order)
```

Guard clauses make the main execution path easier to identify.

### Example: Avoid unnecessary compactness

**Bad:**

```python
valid_users = [u for u in users if u.active and u.age >= 18 and not u.banned]
```

**Good when the filtering rule is important:**

```python
def is_eligible_user(user: User) -> bool:
    return user.is_active and user.age >= 18 and not user.is_banned


valid_users = [user for user in users if is_eligible_user(user)]
```

**Goal:** Readers should be able to follow execution with minimal mental effort.

## 4. Functions and Classes

- Give every function, method, and class a clear, cohesive responsibility.
- Use names that accurately communicate behavior or purpose.
- Keep functions reasonably small and focused, but do not split them merely to reduce line count.
- Extract functions when doing so improves readability, reuse, testability, or separation of responsibilities.
- Prefer explicit inputs and outputs over hidden dependencies or shared mutable state when practical.
- Keep interfaces simple and predictable.
- Use type annotations where supported and appropriate, especially for public interfaces and non-obvious data structures.

### Example: Separate responsibilities

**Bad:**

```python
def process_training_results(metrics):
    save_to_database(metrics)
    send_email_notification(metrics)
    print_metrics(metrics)
    return calculate_average_loss(metrics)
```

This function combines persistence, notification, presentation, and computation.

**Good:**

```python
def calculate_average_loss(losses: list[float]) -> float:
    return sum(losses) / len(losses)


def report_training_results(metrics: TrainingMetrics) -> None:
    save_to_database(metrics)
    send_email_notification(metrics)
    print_metrics(metrics)
```

## 5. Docstrings and Documentation

Document functions, methods, classes, and modules when documentation adds useful information beyond what the code already communicates.

### What to document

For functions and methods, include the following where relevant:
- **Purpose:** What the function does and why it exists.
- **Parameters:** What each parameter represents, including units, formats, constraints, or assumptions.
- **Returns:** What the returned value represents, including meaning, structure, or units.
- **Raises:** Important exceptions and the conditions that cause them.
- **Side effects:** Changes to external state, files, databases, or other observable behavior.
- **Special behavior:** Important edge cases, assumptions, limitations, or non-obvious decisions.

For classes, explain:
- What the class represents.
- Why it exists and what responsibility it encapsulates.
- Important initialization parameters and public attributes.
- Relevant lifecycle behavior, invariants, or constraints.

## 6. Comments

- Add comments for complicated, non-obvious, or easily misunderstood logic.
- Explain **why** something is done when the reason is not apparent from the implementation.
- Explain complex algorithms, mathematical operations, optimization decisions, or unusual implementation constraints when necessary.
- Clarify assumptions, edge cases, and important trade-offs.
- Avoid comments that merely restate the code.

### Example: Explain why, not what

**Bad:**

```python
# Multiply the learning rate by 0.1
learning_rate *= 0.1
```

**Good:**

```python
# Reduce the learning rate after the warmup phase to stabilize
# optimization as training approaches convergence.
learning_rate *= 0.1
```

## 7. Simplicity and Abstraction

- Prefer the simplest implementation that clearly and correctly satisfies requirements.
- Do not introduce abstractions solely for hypothetical future reuse.
- Avoid premature generalization, overengineering, and unnecessary design patterns.
- Reuse existing abstractions when they fit naturally.
- Introduce complexity when it provides a concrete benefit, such as correctness, maintainability, performance, or extensibility.

## 8. Maintainability and Consistency

- Follow existing conventions and architecture unless there is a compelling reason to improve them.
- Maintain consistent naming, formatting, error handling, and organizational patterns.
- Handle errors explicitly where appropriate and make failure behavior predictable.
- Avoid silently swallowing errors or introducing surprising fallback behavior.
- Prefer code that is straightforward to test, debug, and modify.
- When refactoring, preserve existing behavior unless changes are explicitly requested.

## 9. Final Review

Before completing a coding task, review the implementation with these questions:
1. Can another developer understand the code without an explanation from its author?
2. Are names descriptive, consistent, and appropriately concise?
3. Is the control flow straightforward and easy to follow?
4. Are functions and classes focused on meaningful responsibilities?
5. Are abstractions and intermediate variables genuinely useful?
6. Do docstrings explain important contracts, inputs, outputs, and assumptions?
7. Do comments explain non-obvious reasoning rather than repeat the code?
8. Does the implementation follow the language and project's conventions?
9. Is the solution unnecessarily complicated, verbose, or difficult to maintain?
10. Does the code satisfy the requirements without introducing unnecessary changes?

## General Rule

**Optimize for the next human reading the code.**
