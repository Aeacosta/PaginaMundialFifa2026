import { describe, it, expect } from 'vitest';
import { render, screen } from '../../../test/utils';
import { Loading } from '../Loading';

describe('Loading', () => {
  it('renders with default props', () => {
    render(<Loading />);
    
    expect(screen.getByText('Loading...')).toBeInTheDocument();
    expect(screen.getByRole('progressbar')).toBeInTheDocument();
  });

  it('renders with custom message', () => {
    render(<Loading message="Please wait..." />);
    
    expect(screen.getByText('Please wait...')).toBeInTheDocument();
  });

  it('does not render message when message is empty', () => {
    render(<Loading message="" />);
    
    expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
  });

  it('renders in fullScreen mode', () => {
    const { container } = render(<Loading fullScreen />);
    
    const fullScreenBox = container.querySelector('[style*="min-height: 100vh"]');
    expect(fullScreenBox).toBeInTheDocument();
  });

  it('renders in normal mode by default', () => {
    const { container } = render(<Loading />);
    
    const normalBox = container.querySelector('[style*="min-height: 200px"]');
    expect(normalBox).toBeInTheDocument();
  });

  it('renders CircularProgress with custom size', () => {
    render(<Loading size={60} />);
    
    const progressbar = screen.getByRole('progressbar');
    expect(progressbar).toBeInTheDocument();
  });
});

// Made with Bob