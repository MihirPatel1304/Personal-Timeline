export interface TimelineEntry {
  id: number;
  userId: number;
  title: string;
  description: string;
  eventDate: string;
  entryType: "Achievement" | "Activity" | "Milestone" | "Memory";
  category: string;
  imageUrl: string;
  externalUrl: string;
  sourceApi: string;
  externalId: string;
  metadata: string;
  createdAt: string;
  updatedAt: string;
  user?: null;
}

export interface CreateTimelineEntry {
  userId: number;
  title: string;
  description: string;
  eventDate: string;
  entryType: "Achievement" | "Activity" | "Milestone" | "Memory";
  category: string;
  imageUrl?: string;
  externalUrl?: string;
}

export interface TimelineStats {
  totalEntries: number;
  entriesByType: Array<{ entryType: string; count: number }>;
  entriesByCategory: Array<{ category: string; count: number }>;
}

export interface TimelineFilter {
  query?: string;
  category?: string;
  entryType?: string;
  userId: number;
}
