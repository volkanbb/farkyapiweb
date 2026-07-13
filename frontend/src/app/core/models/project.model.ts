export interface Project {
  id: string;
  title: string;
  slug: string;
  shortDescription?: string;
  description?: string;
  client?: string;
  location?: string;
  area?: string;
  projectYear?: number;
  projectDate?: Date | string;
  status: number;
  mainImageUrl?: string;
  coverImageUrl?: string;
  categoryId?: string;
  categoryName?: string;
  category?: any;
  projectImages?: ProjectImage[];
}

export interface ProjectImage {
  id: string;
  imageUrl: string;
  projectId: string;
}
