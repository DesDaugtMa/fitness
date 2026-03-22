---
type: todo
created: 2026-03-22
updated: 2026-03-22
prio: medium
status: open
area: testing
tags: [todo, test, image-management, cmp/image-service, prio/medium, status/todo]
related_feature: [[../features/2026-03-22-image-management]]
related_test: [[../tests/unit/test-image-management]]
---

# Create Image Management Tests

## Description

Implement tests for image upload, storage, retrieval, and deletion.

## Subtasks

### Unit Tests
- [ ] Image model validation (FileData required)
- [ ] MIME type detection and storage
- [ ] File size tracking accuracy

### Integration Tests
- [ ] ImagesController.Create – file upload form submission
- [ ] ImagesController.Create – MIME type detection
- [ ] ImagesController.Create – file size recording
- [ ] ImagesController.Get – serve image with correct Content-Type
- [ ] ImagesController.Get – return FileData in response body
- [ ] ImagesController.Delete – soft delete image
- [ ] Cascade delete handling (Exercise.ImageId, User.ProfileImageId)

### E2E Tests
- [ ] Upload exercise image → appears in exercise details → download
- [ ] Upload profile picture → displays on user profile → replace with new image
- [ ] Image deletion → exercise image reference handling

## Acceptance Criteria

- [ ] ≥80% coverage for Image model and ImagesController
- [ ] File upload/download round-trip validated
- [ ] Correct HTTP headers (Content-Type) on image download
- [ ] All tests passing and integrated into CI/CD

## Performance Notes

- Monitor database size with large image volumes (>1GB)
- Consider Azure Blob Storage migration if needed
- Lazy-load FileData column in queries

## Related

- [[../features/2026-03-22-image-management]] – Implementation
- [[../tests/unit/test-image-management]] – Test documentation

[[../todo/index]]
