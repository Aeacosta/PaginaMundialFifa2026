# Frontend Testing Guide

This document provides information about the testing setup and how to run tests for the World Cup 2026 frontend application.

## Testing Stack

- **Vitest**: Fast unit test framework powered by Vite
- **React Testing Library**: Testing utilities for React components
- **@testing-library/jest-dom**: Custom matchers for DOM assertions
- **@testing-library/user-event**: User interaction simulation

## Project Structure

```
frontend/src/
├── test/
│   ├── setup.ts              # Test environment setup
│   ├── utils.tsx             # Custom render function with providers
│   └── mockData.ts           # Mock data for tests
├── components/
│   └── shared/
│       └── __tests__/        # Component tests
├── services/
│   └── api/
│       └── __tests__/        # API service tests
└── pages/
    └── [PageName]/
        └── __tests__/        # Page component tests
```

## Running Tests

### Install Dependencies

First, install the testing dependencies:

```bash
npm install
```

### Run All Tests

```bash
npm test
```

### Run Tests in Watch Mode

```bash
npm test -- --watch
```

### Run Tests with UI

```bash
npm run test:ui
```

### Run Tests with Coverage

```bash
npm run test:coverage
```

## Writing Tests

### Component Tests

Component tests are located in `__tests__` folders next to the components they test.

Example:
```typescript
import { describe, it, expect } from 'vitest';
import { render, screen } from '../../../test/utils';
import { ErrorMessage } from '../ErrorMessage';

describe('ErrorMessage', () => {
  it('renders with default props', () => {
    render(<ErrorMessage />);
    
    expect(screen.getByText('Error')).toBeInTheDocument();
  });
});
```

### API Service Tests

API service tests mock the axios client and verify correct API calls.

Example:
```typescript
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { teamsService } from '../teams.service';
import { apiClient } from '../../../config/api';

vi.mock('../../../config/api', () => ({
  apiClient: {
    get: vi.fn(),
  },
}));

describe('teamsService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('fetches all teams', async () => {
    vi.mocked(apiClient.get).mockResolvedValue({ data: mockTeams });
    
    const result = await teamsService.getAll();
    
    expect(apiClient.get).toHaveBeenCalledWith('/Teams', {
      params: { pageNumber: 1, pageSize: 10 },
    });
  });
});
```

### Page Tests

Page tests verify the integration of components with React Query hooks.

Example:
```typescript
import { describe, it, expect, vi } from 'vitest';
import { render, screen, waitFor } from '../../../test/utils';
import TeamsPage from '../TeamsPage';
import { teamsService } from '../../../services/api/teams.service';

vi.mock('../../../services/api/teams.service');

describe('TeamsPage', () => {
  it('renders teams list', async () => {
    vi.mocked(teamsService.getAll).mockResolvedValue(mockPagedTeams);
    
    render(<TeamsPage />);
    
    await waitFor(() => {
      expect(screen.getByText('Brazil')).toBeInTheDocument();
    });
  });
});
```

## Test Utilities

### Custom Render

The custom `render` function from `test/utils.tsx` wraps components with necessary providers:
- QueryClientProvider (React Query)
- BrowserRouter (React Router)
- ThemeProvider (Material-UI)

### Mock Data

Common mock data is available in `test/mockData.ts`:
- `mockTeam`, `mockTeams`, `mockPagedTeams`
- `mockStadium`, `mockStadiums`, `mockPagedStadiums`
- `mockMatch`, `mockMatches`, `mockPagedMatches`
- `mockStanding`, `mockStandings`
- `mockDashboard`

## Best Practices

1. **Test Behavior, Not Implementation**: Focus on what the user sees and does
2. **Use Testing Library Queries**: Prefer `getByRole`, `getByLabelText` over `getByTestId`
3. **Mock External Dependencies**: Mock API calls, not internal functions
4. **Clean Up**: Use `beforeEach` to reset mocks between tests
5. **Async Testing**: Use `waitFor` for async operations
6. **Accessibility**: Test with semantic queries to ensure accessibility

## Coverage Goals

- **Components**: Aim for 80%+ coverage
- **Services**: Aim for 90%+ coverage
- **Critical Paths**: 100% coverage for authentication, data mutations

## Troubleshooting

### TypeScript Errors

The TypeScript errors you see before running `npm install` are expected. They will be resolved once the testing dependencies are installed.

### Tests Timing Out

If tests timeout, check:
1. Mock implementations are returning resolved promises
2. React Query cache is being cleared between tests
3. No infinite loops in useEffect hooks

### Mock Not Working

Ensure mocks are defined before imports:
```typescript
vi.mock('../service', () => ({
  service: { method: vi.fn() }
}));
```

## Additional Resources

- [Vitest Documentation](https://vitest.dev/)
- [React Testing Library](https://testing-library.com/react)
- [Testing Library Queries](https://testing-library.com/docs/queries/about)

## Made with Bob