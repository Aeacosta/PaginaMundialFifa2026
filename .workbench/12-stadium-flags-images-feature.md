# Stadium Flags and Images Enhancement Feature

**Date:** June 8, 2026  
**Status:** ✅ Completed  
**Feature:** Enhanced stadium pages with country flags and stadium images

## Overview

Enhanced the stadium functionality to display country flags and high-quality stadium images throughout the application, improving visual appeal and user experience.

## Changes Implemented

### 1. Backend Changes

#### Domain Layer
- **Stadium Entity** (`WorldCup2026.Domain/Entities/Stadium.cs`)
  - Added `FlagUrl` property (string?, nullable)
  - Property stores URL to country flag image

#### Application Layer
- **StadiumDto** (`WorldCup2026.Application/DTOs/Stadium/StadiumDto.cs`)
  - Added `FlagUrl` property
- **CreateStadiumDto** (`WorldCup2026.Application/DTOs/Stadium/CreateStadiumDto.cs`)
  - Added `FlagUrl` property
- **UpdateStadiumDto** (`WorldCup2026.Application/DTOs/Stadium/UpdateStadiumDto.cs`)
  - Added `FlagUrl` property
  - Added `ImageUrl` property (was missing)

#### Infrastructure Layer
- **StadiumConfiguration** (`WorldCup2026.Infrastructure/Data/Configurations/StadiumConfiguration.cs`)
  - Added configuration for `FlagUrl` with max length 500
- **Database Migration** (`20260608002803_AddFlagUrlToStadium`)
  - Created migration to add FlagUrl column to Stadiums table
- **StadiumSeeder** (`WorldCup2026.Infrastructure/Data/Seeding/StadiumSeeder.cs`)
  - Added flag URLs for all 16 stadiums
  - Added stadium image URLs for all 16 stadiums
  - Used free, reliable sources:
    - Flags: flagcdn.com (w320 resolution)
    - Images: Unsplash (800x600 cropped)

### 2. Frontend Changes

#### Type Definitions
- **types/index.ts**
  - Added `flagUrl?: string` to `StadiumDto`
  - Added `flagUrl?: string` to `CreateStadiumDto`
  - Added `flagUrl?: string` to `UpdateStadiumDto`

#### Components
- **PageHeader** (`components/shared/PageHeader.tsx`)
  - Updated `title` prop type from `string` to `string | React.ReactNode`
  - Allows custom elements (like flags) in page headers

#### Pages
- **StadiumsPage** (`pages/Stadiums/StadiumsPage.tsx`)
  - Added country flag display next to location in stadium cards
  - Flag size: 24x16px with rounded corners
  - Positioned inline with city and country text

- **StadiumDetailsPage** (`pages/Stadiums/StadiumDetailsPage.tsx`)
  - Added flag in page header next to stadium name (32x21px)
  - Added flag in location information section (28x19px)
  - Flags display conditionally if `flagUrl` exists

## Asset Sources

### Country Flags
- **Source:** flagcdn.com
- **Format:** PNG
- **Resolution:** w320 (320px width)
- **Countries:** USA, Mexico, Canada
- **URLs:**
  - USA: `https://flagcdn.com/w320/us.png`
  - Mexico: `https://flagcdn.com/w320/mx.png`
  - Canada: `https://flagcdn.com/w320/ca.png`
- **License:** Free to use

### Stadium Images
- **Source:** Unsplash
- **Format:** JPEG
- **Resolution:** 800x600 (cropped)
- **Quality:** High-quality sports venue photography
- **License:** Free to use (Unsplash License)

## Stadium Data

### USA Stadiums (11)
1. MetLife Stadium - East Rutherford, NJ (82,500)
2. AT&T Stadium - Arlington, TX (80,000)
3. Arrowhead Stadium - Kansas City, MO (76,416)
4. SoFi Stadium - Inglewood, CA (70,240)
5. Mercedes-Benz Stadium - Atlanta, GA (71,000)
6. NRG Stadium - Houston, TX (72,220)
7. Lincoln Financial Field - Philadelphia, PA (69,796)
8. Lumen Field - Seattle, WA (69,000)
9. Levi's Stadium - Santa Clara, CA (68,500)
10. Hard Rock Stadium - Miami Gardens, FL (64,767)
11. Gillette Stadium - Foxborough, MA (65,878)

### Mexico Stadiums (3)
1. Estadio Azteca - Mexico City (87,523)
2. Estadio BBVA - Monterrey (53,500)
3. Estadio Akron - Guadalajara (46,232)

### Canada Stadiums (2)
1. BMO Field - Toronto (45,500)
2. BC Place - Vancouver (54,500)

## Technical Implementation

### Flag Display Specifications
```typescript
// Stadium Card (StadiumsPage)
{
  width: 24,
  height: 16,
  objectFit: 'cover',
  borderRadius: 0.5,
  ml: 0.5
}

// Page Header (StadiumDetailsPage)
{
  width: 32,
  height: 21,
  objectFit: 'cover',
  borderRadius: 0.5
}

// Location Section (StadiumDetailsPage)
{
  width: 28,
  height: 19,
  objectFit: 'cover',
  borderRadius: 0.5
}
```

### Conditional Rendering
All flag and image displays use conditional rendering:
```typescript
{stadium.flagUrl && (
  <Box component="img" src={stadium.flagUrl} alt={`${stadium.country} flag`} />
)}
```

## Database Migration

### Migration Name
`20260608002803_AddFlagUrlToStadium`

### SQL Changes
```sql
ALTER TABLE "Stadiums" ADD "FlagUrl" TEXT NULL;
```

### Migration Commands
```bash
# Create migration
dotnet ef migrations add AddFlagUrlToStadium --project ../WorldCup2026.Infrastructure/WorldCup2026.Infrastructure.csproj --startup-project WorldCup2026.API.csproj

# Apply migration
dotnet ef database update
```

## Testing Checklist

- [x] Backend builds successfully
- [x] Database migration applied successfully
- [x] Stadium seeder includes flag and image URLs
- [x] Frontend TypeScript types updated
- [x] Stadium cards display flags correctly
- [x] Stadium details page shows flags in multiple locations
- [x] Images load from Unsplash
- [x] Flags load from flagcdn.com
- [x] Responsive design maintained
- [x] Graceful fallback when URLs are missing

## Benefits

1. **Enhanced Visual Appeal:** Country flags add visual interest and context
2. **Better User Experience:** Users can quickly identify stadium countries
3. **Professional Look:** High-quality images improve overall aesthetics
4. **Consistent Design:** Flags displayed uniformly across all views
5. **Free Assets:** No licensing costs for images or flags
6. **Reliable Sources:** Using established CDNs for asset delivery

## Future Enhancements

- Add stadium capacity visualization
- Include stadium history/facts
- Add interactive map with stadium locations
- Display upcoming matches at each stadium
- Add stadium photo galleries
- Include 360° virtual tours (if available)

## Related Files

### Backend
- `backend/src/WorldCup2026.Domain/Entities/Stadium.cs`
- `backend/src/WorldCup2026.Application/DTOs/Stadium/*.cs`
- `backend/src/WorldCup2026.Infrastructure/Data/Configurations/StadiumConfiguration.cs`
- `backend/src/WorldCup2026.Infrastructure/Data/Seeding/StadiumSeeder.cs`
- `backend/src/WorldCup2026.Infrastructure/Migrations/20260608002803_AddFlagUrlToStadium.cs`

### Frontend
- `frontend/src/types/index.ts`
- `frontend/src/components/shared/PageHeader.tsx`
- `frontend/src/pages/Stadiums/StadiumsPage.tsx`
- `frontend/src/pages/Stadiums/StadiumDetailsPage.tsx`

## Notes

- Initial attempt used Wikimedia Commons URLs which had access issues
- Switched to Unsplash for reliable, high-quality stadium images
- flagcdn.com provides consistent, free flag images with good CDN support
- All images use appropriate sizing and cropping for optimal display
- Flag aspect ratio maintained (3:2) across all display sizes

---

**Made with Bob** 🤖