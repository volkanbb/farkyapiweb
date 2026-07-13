export interface Service {
  id: string;
  title: string;
  slug: string;
  shortDescription?: string;
  description?: string;
  iconName?: string;
  imageUrl?: string;
  sortOrder: number;
}
