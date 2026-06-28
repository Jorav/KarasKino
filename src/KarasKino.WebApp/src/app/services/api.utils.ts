import { environment } from '../../environments/environment';

export function absoluteApiUrl(path: string = ''): string {
  const base = environment.apiBaseUrl;
  const absolute = base.startsWith('http') ? base : `${window.location.origin}${base}`;
  return path ? `${absolute}/${path}` : absolute;
}
