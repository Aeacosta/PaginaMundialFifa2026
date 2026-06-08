import { Card, CardContent, Typography, Box, Skeleton, Zoom, alpha } from '@mui/material';
import TrendingUpIcon from '@mui/icons-material/TrendingUp';
import TrendingDownIcon from '@mui/icons-material/TrendingDown';

interface StatCardProps {
  title: string;
  value: string | number;
  icon?: React.ReactNode;
  trend?: {
    value: number;
    isPositive: boolean;
  };
  loading?: boolean;
  color?: 'primary' | 'secondary' | 'success' | 'error' | 'warning' | 'info';
}

export const StatCard: React.FC<StatCardProps> = ({
  title,
  value,
  icon,
  trend,
  loading = false,
  color = 'primary',
}) => {
  if (loading) {
    return (
      <Card>
        <CardContent>
          <Skeleton variant="text" width="60%" height={24} />
          <Skeleton variant="text" width="40%" height={48} sx={{ mt: 1 }} />
        </CardContent>
      </Card>
    );
  }

  return (
    <Zoom in timeout={500}>
      <Card
        sx={(theme) => ({
          height: '100%',
          position: 'relative',
          overflow: 'hidden',
          background: `linear-gradient(135deg, ${alpha(theme.palette[color].main, 0.05)} 0%, transparent 100%)`,
          border: '1px solid',
          borderColor: 'divider',
          transition: 'all 0.3s ease-in-out',
          '&:hover': {
            transform: 'translateY(-8px) scale(1.02)',
            boxShadow: `0 12px 32px ${alpha('#000', 0.5)}`,
            borderColor: `${color}.main`,
            '& .stat-icon': {
              transform: 'scale(1.2) rotate(5deg)',
            },
            '& .stat-value': {
              color: `${color}.main`,
            },
          },
          '&::before': {
            content: '""',
            position: 'absolute',
            top: 0,
            left: 0,
            right: 0,
            height: '4px',
            background: `linear-gradient(90deg, ${theme.palette[color].main}, ${theme.palette[color].light})`,
            opacity: 0,
            transition: 'opacity 0.3s ease-in-out',
          },
          '&:hover::before': {
            opacity: 1,
          },
        })}
      >
        <CardContent>
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'flex-start',
              mb: 2,
            }}
          >
            <Typography
              color="text.secondary"
              variant="body2"
              gutterBottom
              sx={{ fontWeight: 500, textTransform: 'uppercase', letterSpacing: '0.5px' }}
            >
              {title}
            </Typography>
            {icon && (
              <Box
                className="stat-icon"
                sx={{
                  color: `${color}.main`,
                  display: 'flex',
                  alignItems: 'center',
                  transition: 'all 0.3s ease-in-out',
                  opacity: 0.8,
                }}
              >
                {icon}
              </Box>
            )}
          </Box>

          <Typography
            className="stat-value"
            variant="h4"
            component="div"
            sx={{
              mb: 1,
              fontWeight: 700,
              transition: 'color 0.3s ease-in-out',
            }}
          >
            {value}
          </Typography>

          {trend && (
            <Box
              sx={{
                display: 'flex',
                alignItems: 'center',
                gap: 0.5,
                color: trend.isPositive ? 'success.main' : 'error.main',
                animation: 'fadeIn 0.5s ease-in-out',
                '@keyframes fadeIn': {
                  from: { opacity: 0, transform: 'translateY(5px)' },
                  to: { opacity: 1, transform: 'translateY(0)' },
                },
              }}
            >
              {trend.isPositive ? (
                <TrendingUpIcon fontSize="small" />
              ) : (
                <TrendingDownIcon fontSize="small" />
              )}
              <Typography variant="body2" sx={{ fontWeight: 600 }}>
                {Math.abs(trend.value)}%
              </Typography>
            </Box>
          )}
        </CardContent>
      </Card>
    </Zoom>
  );
};

// Made with Bob
