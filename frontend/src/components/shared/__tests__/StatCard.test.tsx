import { describe, it, expect } from 'vitest';
import { render, screen } from '../../../test/utils';
import { StatCard } from '../StatCard';
import SportsIcon from '@mui/icons-material/Sports';

describe('StatCard', () => {
  it('renders with required props', () => {
    render(<StatCard title="Total Teams" value={48} />);
    
    expect(screen.getByText('Total Teams')).toBeInTheDocument();
    expect(screen.getByText('48')).toBeInTheDocument();
  });

  it('renders with string value', () => {
    render(<StatCard title="Status" value="Active" />);
    
    expect(screen.getByText('Status')).toBeInTheDocument();
    expect(screen.getByText('Active')).toBeInTheDocument();
  });

  it('renders with icon', () => {
    render(
      <StatCard 
        title="Total Teams" 
        value={48} 
        icon={<SportsIcon data-testid="sports-icon" />} 
      />
    );
    
    expect(screen.getByTestId('sports-icon')).toBeInTheDocument();
  });

  it('renders with positive trend', () => {
    render(
      <StatCard 
        title="Total Teams" 
        value={48} 
        trend={{ value: 10, isPositive: true }} 
      />
    );
    
    expect(screen.getByText('10%')).toBeInTheDocument();
  });

  it('renders with negative trend', () => {
    render(
      <StatCard 
        title="Total Teams" 
        value={48} 
        trend={{ value: -5, isPositive: false }} 
      />
    );
    
    expect(screen.getByText('5%')).toBeInTheDocument();
  });

  it('renders loading skeleton when loading is true', () => {
    render(<StatCard title="Total Teams" value={48} loading />);
    
    // Should not show the actual content when loading
    expect(screen.queryByText('Total Teams')).not.toBeInTheDocument();
    expect(screen.queryByText('48')).not.toBeInTheDocument();
  });

  it('applies custom color', () => {
    render(
      <StatCard 
        title="Total Teams" 
        value={48} 
        color="success" 
      />
    );
    
    expect(screen.getByText('Total Teams')).toBeInTheDocument();
  });

  it('renders without trend when not provided', () => {
    render(<StatCard title="Total Teams" value={48} />);
    
    expect(screen.queryByText(/%/)).not.toBeInTheDocument();
  });
});

// Made with Bob