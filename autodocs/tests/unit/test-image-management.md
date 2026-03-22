---
type: test
created: 2026-03-22
area: media
tags: [test, unit, integration, media, cmp/image-service, status/todo]
related_feature: [[../features/2026-03-22-image-management]]
test_level: [unit, integration]
coverage_status: 0%
---

# Image Management Tests

## Overview

Tests for image upload, storage, and retrieval.

## Test Areas

### Unit Tests (Pending)
- [ ] Image model validation
  - FileData required
  - MIME type validation
  - File size tracking

### Integration Tests (Pending)
- [ ] ImagesController.Create (upload)
  - File upload form submission
  - MIME type detection
  - File size recording
  - Uploader association
- [ ] ImagesController.Get (download/serve)
  - Serve image with correct Content-Type header
  - Return FileData as response body
  - Handle 404 for non-existent images
- [ ] ImagesController.Delete
  - Mark image as deleted (soft delete recommended)
  - Cascade to Exercise/User references
  - Prevent orphaned image references

### E2E Tests (Pending)
- [ ] Upload exercise image
  - Select .jpg/.png file
  - Submit form
  - Image appears in exercise details
  - Download and verify content
- [ ] Profile picture upload
  - Upload image for user account
  - Display on profile page
  - Replace with new image

## Performance Notes

- Lazy-load FileData to avoid full-table scans
- Consider bulk soft-delete for storage cleanup
- Monitor database size if images exceed 1GB

## Related

- [[../features/2026-03-22-image-management]] – Image management implementation
- [[../todo/todo-create-image-management-tests]] – Test implementation backlog

[[../tests/index]]
