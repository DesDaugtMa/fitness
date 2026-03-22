---
type: test
created: 2026-03-22
area: workout
tags: [test, unit, integration, workout, cmp/workout-service, status/todo]
related_feature: [[../features/2026-03-21-workout-tracking]]
test_level: [unit, integration]
coverage_status: 0%
---

# Workout Tracking Tests

## Overview

Tests for exercise management, training plans, and workout session tracking.

## Test Areas

### Unit Tests (Pending)
- [ ] Exercise model validation
  - Name required
  - Multiple muscle groups
  - Image reference
- [ ] TrainingPlan model validation
  - User association
  - Daily schedule
- [ ] WorkoutSession model validation
  - Start and complete timestamps
  - Logged exercises
- [ ] WorkoutLog calculations
  - Actual vs. planned performance

### Integration Tests (Pending)
- [ ] ExercisesController.Create
  - Create exercise with muscle groups
  - Assign to user (Creator)
  - Validate public/private sharing
- [ ] TrainingPlansController operations
  - Create plan with daily schedule
  - Assign exercises to days
  - Modify plan (add/remove days)
  - Delete plan
- [ ] WorkoutSession lifecycle
  - Start session (startedAt)
  - Log exercises during session
  - Complete session (completedAt)
  - Historical queries

### E2E Tests (Pending)
- [ ] Complete workout session flow
  - View saved plans
  - Start new session from plan
  - Log each exercise with actual sets/reps
  - Complete session
  - View session history
- [ ] Training plan performance analytics
  - Compare actual vs. planned
  - Track progress over time

## Related

- [[../features/2026-03-21-workout-tracking]] – Workout tracking implementation
- [[../todo/todo-create-workout-tracking-tests]] – Test implementation backlog

[[../tests/index]]
