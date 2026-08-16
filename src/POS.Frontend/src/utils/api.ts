export const API_BASE_URL = import.meta.env.VITE_API_URL || '/api';

export class ApiError extends Error {
  status: number;
  constructor(status: number, message: string) {
    super(message);
    this.status = status;
    this.name = 'ApiError';
  }
}

export async function fetchApi<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
  const url = `${API_BASE_URL}${endpoint}`;
  const token = localStorage.getItem('auth_token');
  const headers = {
    'Content-Type': 'application/json',
    ...(token ? { 'Authorization': `Bearer ${token}` } : {}),
    ...options.headers,
  };

  const response = await fetch(url, { ...options, headers });

  if (!response.ok) {
    const errorText = await response.text();
    let errorMessage = errorText || response.statusText;
    try {
      if (errorText) {
        const parsed = JSON.parse(errorText);
        if (parsed.error) {
          errorMessage = parsed.error;
        } else if (parsed.errors && typeof parsed.errors === 'object') {
          const errorsList = Object.values(parsed.errors).flat();
          errorMessage = errorsList.length > 0 ? errorsList.join(' ') : (parsed.title || parsed.message);
        } else if (parsed.message) {
          errorMessage = parsed.message;
        } else if (parsed.title) {
          errorMessage = parsed.title;
        }
      }
    } catch (e) {
      // Ignore if not JSON
    }
    throw new ApiError(response.status, errorMessage);
  }

  const text = await response.text();
  if (!text) {
    return {} as T;
  }
  try {
    return JSON.parse(text);
  } catch {
    return text as unknown as T;
  }
}
