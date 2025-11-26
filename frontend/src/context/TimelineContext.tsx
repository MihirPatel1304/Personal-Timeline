/* eslint-disable react-refresh/only-export-components */
import { createContext, useState, useEffect, useCallback } from "react";
import type { ReactNode } from "react";
import type {
  TimelineEntry,
  CreateTimelineEntry,
  TimelineStats,
} from "../types/TimelineEntry";
import { timelineService } from "../services/timelineService";
import { useAuth } from "../hooks/useAuth"; // CHANGE THIS LINE - import from hooks folder

export interface TimelineContextType {
  // Add 'export' here
  entries: TimelineEntry[];
  stats: TimelineStats | null;
  isLoading: boolean;
  error: string | null;
  fetchEntries: () => Promise<void>;
  fetchStats: () => Promise<void>;
  createEntry: (entry: CreateTimelineEntry) => Promise<TimelineEntry>;
  updateEntry: (
    id: number,
    entry: Partial<TimelineEntry>
  ) => Promise<TimelineEntry>;
  deleteEntry: (id: number) => Promise<void>;
  searchEntries: (
    query: string,
    category?: string,
    entryType?: string
  ) => Promise<void>;
}

export const TimelineContext = createContext<TimelineContextType | undefined>( // Add 'export' here
  undefined
);

export const TimelineProvider = ({ children }: { children: ReactNode }) => {
  const { user } = useAuth();
  const [entries, setEntries] = useState<TimelineEntry[]>([]);
  const [stats, setStats] = useState<TimelineStats | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchEntries = useCallback(async () => {
    if (!user) return;

    setIsLoading(true);
    setError(null);
    try {
      const data = await timelineService.getAllEntries(user.id);
      setEntries(data);
    } catch (err) {
      setError("Failed to fetch timeline entries");
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  }, [user]);

  const fetchStats = useCallback(async () => {
    if (!user) return;

    try {
      const data = await timelineService.getStats(user.id);
      setStats(data);
    } catch (err) {
      console.error("Failed to fetch stats:", err);
    }
  }, [user]);

  const createEntry = async (
    entry: CreateTimelineEntry
  ): Promise<TimelineEntry> => {
    setIsLoading(true);
    setError(null);
    try {
      const newEntry = await timelineService.createEntry(entry);
      setEntries((prev) => [newEntry, ...prev]);
      await fetchStats();
      return newEntry;
    } catch (err) {
      setError("Failed to create entry");
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  const updateEntry = async (
    id: number,
    entry: Partial<TimelineEntry>
  ): Promise<TimelineEntry> => {
    setIsLoading(true);
    setError(null);
    try {
      const updatedEntry = await timelineService.updateEntry(id, entry);
      setEntries((prev) => prev.map((e) => (e.id === id ? updatedEntry : e)));
      await fetchStats();
      return updatedEntry;
    } catch (err) {
      setError("Failed to update entry");
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  const deleteEntry = async (id: number): Promise<void> => {
    setIsLoading(true);
    setError(null);
    try {
      await timelineService.deleteEntry(id);
      setEntries((prev) => prev.filter((e) => e.id !== id));
      await fetchStats();
    } catch (err) {
      setError("Failed to delete entry");
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  const searchEntries = async (
    query: string,
    category?: string,
    entryType?: string
  ): Promise<void> => {
    if (!user) return;

    setIsLoading(true);
    setError(null);
    try {
      const data = await timelineService.searchEntries({
        userId: user.id,
        query,
        category,
        entryType,
      });
      setEntries(data);
    } catch (err) {
      setError("Failed to search entries");
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (user) {
      fetchEntries();
      fetchStats();
    }
  }, [user, fetchEntries, fetchStats]);

  const value = {
    entries,
    stats,
    isLoading,
    error,
    fetchEntries,
    fetchStats,
    createEntry,
    updateEntry,
    deleteEntry,
    searchEntries,
  };

  return (
    <TimelineContext.Provider value={value}>
      {children}
    </TimelineContext.Provider>
  );
};
