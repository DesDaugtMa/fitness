---
type: feature
created: 2026-03-21
updated: 2026-03-22
status: released
area: workout
tags: [feature, workout, tracking, exercise, cmp/workout-service, dom/training, status/active]
components: [WorkoutSession, WorkoutLog, TrainingPlan, TrainingDay, PlanExercise, Exercise, ExerciseMuscleGroup]
related_adrs: [adr-001-use-entity-framework]
related_tests: []
related_todos: [todo-create-workout-tracking-tests]
commits: []
---

# Workout Tracking System

## Overview

Comprehensive exercise and workout session management system supporting:

- Custom exercise library (personal and shared)
- Exercise categorization by muscle groups
- Training plan templates with daily schedules
- Workout session tracking and logging
- Exercise performance metrics (sets, reps, weight)
- Workout history and analytics

## Architecture

### Domain Models

**Exercise**
- Name (required)
- Description (optional)
- ImageId (optional, foreign key to Image)
- VideoUrl (optional URL for demonstration)
- IsPublic (boolean for sharing)
- CreatorId (optional, self-referencing to User)
- CreatedAt timestamp
- Navigation: ExerciseMuscleGroups (many-to-many)

**ExerciseMuscleGroup** (Join Table)
- ExerciseId, MuscleGroupId (composite primary key)
- Links exercises to muscle groups they target

**MuscleGroup**
- Name (unique, required)
- Predefined list of muscle groups (Chest, Back, Legs, Arms, etc.)

**TrainingPlan**
- UserId (required, foreign key to User)
- Title (required)
- Description (optional)
- CreatedAt timestamp
- Navigation: TrainingDays (one-to-many)

**TrainingDay**
- PlanId (required, foreign key to TrainingPlan)
- DayOfWeek (e.g., Monday, Tuesday, Wednesday)
- Navigation: PlanExercise (one-to-many)

**PlanExercise**
- ExerciseId, TrainingDayId (foreign keys)
- PlannedSets, PlannedReps, PlannedWeight (optional guidance)
- Navigation: Exercise, TrainingDay

**WorkoutSession**
- UserId (required, foreign key to User)
- PlanId (required, foreign key to TrainingPlan)
- StartedAt (timestamp)
- CompletedAt (nullable, null if in-progress)
- Navigation: WorkoutLogs (one-to-many)

**WorkoutLog**
- SessionId (required, foreign key to WorkoutSession)
- ExerciseId (required, foreign key to Exercise)
- ActualSets, ActualReps, ActualWeight (nullable)
- Notes (optional)
- CompletedAt timestamp

### API Controllers

**ExercisesController** (`Controllers/ExercisesController.cs`)
- GET `/Exercises` – List all exercises (including shared ones)
- GET `/Exercises/Details/{id}` – View exercise details
- POST `/Exercises/Create` – Create new exercise with muscle groups
- DELETE `/Exercises/{id}` – Remove exercise (soft delete recommended)

**TrainingPlansController** (`Controllers/TrainingPlans/*`)
- CRUD operations for training plans
- Daily schedule management
- Muscle group selection for exercises

**MuscleGroupsController** (`Controllers/MuscleGroups/*`)
- GET list of all muscle groups
- CRUD for admin operations

### Key Features

1. **Exercise Library**
   - Personal exercises (CreatorId set)
   - Public/shared exercises (IsPublic flag)
   - Multiple muscle group targeting per exercise
   - Optional video/image demonstration

2. **Training Plans**
   - Weekly structure (TrainingDays mapped to days of week)
   - Exercise assignment with expected sets/reps/weight
   - Reusable plan templates

3. **Workout Sessions**
   - Start/stop timing (StartedAt/CompletedAt)
   - Actual performance vs. planned performance
   - Exercise-by-exercise logging (WorkoutLog entries)
   - Notes for each exercise (form feedback, difficulty notes)

4. **Performance Tracking**
   - Compare ActualSets/Reps vs. PlannedSets/Reps
   - Weight progression tracking
   - History per exercise per user

## Implementation Notes

- Composite keys for ExerciseMuscleGroup and PlanExercise (many-to-many)
- All timestamps use UTC (DateTime.UtcNow)
- WorkoutSession.CompletedAt remains nullable for ongoing sessions
- IsPublic flag enables exercise sharing without copying/version control
- Planned vs. Actual separation allows plan adherence analytics

## Testing

- [ ] Unit tests for Exercise validation (name required, etc.)
- [ ] Integration tests for training plan creation and exercise assignment
- [ ] Workout session start/stop and logging
- [ ] Performance metric calculations (sets/reps progress)
- [ ] Muscle group filtering and multi-group exercises
- [ ] E2E: Complete workout session flow (start → log exercises → complete)

## Related

- [[../adrs/adr-001-use-entity-framework]] – ORM decision
- [[../tests/test-workout-tracking]] – Test documentation (pending)
- [[../todo/todo-create-workout-tracking-tests]] – Test implementation backlog

[[../features/index]]
