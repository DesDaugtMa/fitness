---
type: feature
created: 2026-03-22
status: released
area: media
tags: [feature, media, storage, images, cmp/image-service, dom/media, status/active]
components: [Image, ImagesController]
related_adrs: [adr-001-use-entity-framework]
related_tests: []
related_todos: [todo-create-image-management-tests]
commits: []
---

# Image Management & Storage

## Overview

Image upload, storage, and management system supporting:

- Exercise demonstration images and videos
- User profile pictures
- Image metadata (file type, size, uploader info)
- Browse and delete operations
- MIME type validation

## Architecture

### Domain Models

**Image**
- FileData (byte array, stores actual image content)
- FileSizeBytes (tracks upload size)
- FileExtension (e.g., ".jpg", ".png")
- MimeType (e.g., "image/jpeg", "image/png")
- Description (optional, caption or notes)
- UploaderId (optional, foreign key to User who uploaded)
- CreatedAt timestamp

### API Controllers

**ImagesController** (`Controllers/ImagesController.cs`)
- GET `/Images` – List all images
- GET `/Images/Details/{id}` – View image metadata and preview
- GET `/Images/{id}` – Download/serve image file
- POST `/Images/Create` – Upload new image
  - File upload form with validation
  - MIME type detection and storage
  - File size tracking
- DELETE `/Images/{id}` – Remove image (with optional cascade to references)

### Key Features

1. **In-Database Storage**
   - Images stored as BLOB (byte array) in FileData column
   - File metadata preserved (extension, size, MIME type)
   - CreatedAt tracks upload timestamp

2. **Uploader Tracking**
   - Optional foreign key to User (UploaderId)
   - Supports system/default images with null uploader
   - Enables user-contributed exercise demonstrations

3. **Metadata Management**
   - FileExtension from original upload
   - MimeType for proper content-type headers when serving
   - Description field for captions/notes
   - FileSizeBytes for quota/storage management

4. **Integration Points**
   - Exercise.ImageId → references Image
   - User.ProfileImageId → references Image
   - Replace-only updates (no partial updates to FileData)

## Implementation Notes

- Images stored in database (not external blob storage initially)
- File serving should return proper Content-Type header based on MimeType
- FileData not indexed (full table scan for queries, intentional)
- Consider pagination for image listing if datasets grow large
- Cascade behavior: images soft-deleted if referenced entity is deleted (recommended)

## Testing

- [ ] Unit tests for Image model validation (FileData required, etc.)
- [ ] File upload acceptance and MIME type validation
- [ ] File size tracking accuracy
- [ ] Download/serve image with correct headers
- [ ] Delete cascades to Exercise/User references
- [ ] E2E: Upload exercise image → assign to exercise → download image

## Performance Considerations

- Database-backed storage may impact query performance with large binary data
- Consider transferring to Azure Blob Storage or S3 if images exceed ~1GB total
- Lazy-load FileData column (use .AsNoTracking() for lists, explicit Include() for single download)

## Related

- [[../features/2026-03-21-workout-tracking]] – Exercise images
- [[../features/2026-03-21-user-management]] – Profile pictures
- [[../adrs/adr-001-use-entity-framework]] – ORM decision
- [[../tests/test-image-management]] – Test documentation (pending)
- [[../todo/todo-create-image-management-tests]] – Test implementation backlog

[[../features/index]]
