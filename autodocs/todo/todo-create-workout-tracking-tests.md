---
type: todo
created: 2026-03-22
updated: 2026-03-22
prio: high
status: open
area: testing
tags: [todo, test, workout-tracking, cmp/workout-service, prio/high, status/todo]
related_feature: [[../features/2026-03-21-workout-tracking]]
related_test: [[../tests/unit/test-workout-tracking]]
---

# Create Workout Tracking Tests

## Description

Implement unit and integration tests for the Workout Tracking System.

## Subtasks

### Unit Tests
- [ ] Exercise model validation (Name required)
- [ ] Exercise.IsPublic flag behavior
- [ ] MuscleGroup validation
- [ ] TrainingPlan validation (UserId required)
- [ ] TrainingDay schedule creation
- [ ] WorkoutSession duration calculation
- [ ] WorkoutLog actual vs. planned performance

### Integration Tests
- [ ] ExercisesController.Create – new exercise with muscle groups
- [ ] ExercisesController.Index – list exercises (public + user's)
- [ ] ExercisesController.Details – view exercise with muscle groups
- [ ] TrainingPlansController.Create – create plan with days
- [ ] TrainingPlansController – assign exercises to days
- [ ] WorkoutSession lifecycle (start → log → complete)
- [ ] WorkoutLog creation during session

### E2E Tests
- [ ] Create exercise → assign to training plan → view in plan
- [ ] Start workout session → log exercises → complete session
- [ ] View workout history with performance metrics
- [ ] Training plan progress (planned vs. actual)

## Acceptance Criteria

- [ ] ≥80% coverage for Exercise/TrainingPlan/WorkoutSession models
- [ ] Integration tests for all controller CRUD operations
- [ ] Performance tracking (sets/reps/weight) validated
- [ ] All tests passing and integrated into CI/CD

## Related

- [[../features/2026-03-21-workout-tracking]] – Implementation
- [[../tests/unit/test-workout-tracking]] – Test documentation

[[../todo/index]]
