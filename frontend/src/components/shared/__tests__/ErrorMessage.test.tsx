import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '../../../test/utils';
import userEvent from '@testing-library/user-event';
import { ErrorMessage } from '../ErrorMessage';

describe('ErrorMessage', () => {
  it('renders with default props', () => {
    render(<ErrorMessage />);
    
    expect(screen.getByText('Error')).toBeInTheDocument();
    expect(screen.getByText('An error occurred while loading data.')).toBeInTheDocument();
  });

  it('renders with custom message and title', () => {
    render(
      <ErrorMessage 
        title="Custom Error" 
        message="Something went wrong!" 
      />
    );
    
    expect(screen.getByText('Custom Error')).toBeInTheDocument();
    expect(screen.getByText('Something went wrong!')).toBeInTheDocument();
  });

  it('renders retry button when onRetry is provided', () => {
    const onRetry = vi.fn();
    render(<ErrorMessage onRetry={onRetry} />);
    
    const retryButton = screen.getByRole('button', { name: /retry/i });
    expect(retryButton).toBeInTheDocument();
  });

  it('calls onRetry when retry button is clicked', async () => {
    const user = userEvent.setup();
    const onRetry = vi.fn();
    render(<ErrorMessage onRetry={onRetry} />);
    
    const retryButton = screen.getByRole('button', { name: /retry/i });
    await user.click(retryButton);
    
    expect(onRetry).toHaveBeenCalledTimes(1);
  });

  it('does not render retry button when onRetry is not provided', () => {
    render(<ErrorMessage />);
    
    const retryButton = screen.queryByRole('button', { name: /retry/i });
    expect(retryButton).not.toBeInTheDocument();
  });

  it('renders in fullScreen mode', () => {
    const { container } = render(<ErrorMessage fullScreen />);
    
    const fullScreenBox = container.querySelector('[style*="min-height: 100vh"]');
    expect(fullScreenBox).toBeInTheDocument();
  });

  it('renders in normal mode by default', () => {
    const { container } = render(<ErrorMessage />);
    
    const normalBox = container.querySelector('[style*="min-height: 100vh"]');
    expect(normalBox).not.toBeInTheDocument();
  });
});

// Made with Bob