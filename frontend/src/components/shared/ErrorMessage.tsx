import { Alert, AlertTitle, Box, Button } from '@mui/material';
import RefreshIcon from '@mui/icons-material/Refresh';

interface ErrorMessageProps {
  message?: string;
  title?: string;
  onRetry?: () => void;
  fullScreen?: boolean;
}

export const ErrorMessage: React.FC<ErrorMessageProps> = ({
  message = 'An error occurred while loading data.',
  title = 'Error',
  onRetry,
  fullScreen = false,
}) => {
  const content = (
    <Alert 
      severity="error"
      action={
        onRetry && (
          <Button
            color="inherit"
            size="small"
            startIcon={<RefreshIcon />}
            onClick={onRetry}
          >
            Retry
          </Button>
        )
      }
    >
      <AlertTitle>{title}</AlertTitle>
      {message}
    </Alert>
  );

  if (fullScreen) {
    return (
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          minHeight: '100vh',
          p: 3,
        }}
      >
        <Box sx={{ maxWidth: 600, width: '100%' }}>
          {content}
        </Box>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 2 }}>
      {content}
    </Box>
  );
};

// Made with Bob
