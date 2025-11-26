import { useTimeline } from "../../hooks/useTimeline";
import { useAuth } from "../../hooks/useAuth";
import { TimelineEntry } from "../timeline/TimelineEntry";
import { useState } from "react";
import { EntryModal } from "../timeline/EntryModal";
import { EntryForm } from "../timeline/EntryForm";
import type {
  TimelineEntry as TimelineEntryType,
  CreateTimelineEntry,
} from "../../types/TimelineEntry";

export const Dashboard = () => {
  const { user } = useAuth();
  const { entries, stats, isLoading, createEntry, updateEntry, deleteEntry } =
    useTimeline();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingEntry, setEditingEntry] = useState<TimelineEntryType | null>(
    null
  );

  const recentEntries = entries.slice(0, 5);

  const handleCreateEntry = async (entry: CreateTimelineEntry) => {
    await createEntry(entry);
    setIsModalOpen(false);
  };

  const handleUpdateEntry = async (entry: Partial<TimelineEntryType>) => {
    if (editingEntry) {
      await updateEntry(editingEntry.id, entry);
      setEditingEntry(null);
      setIsModalOpen(false);
    }
  };

  const handleDeleteEntry = async (id: number) => {
    if (window.confirm("Are you sure you want to delete this entry?")) {
      await deleteEntry(id);
    }
  };

  const handleEditClick = (entry: TimelineEntryType) => {
    setEditingEntry(entry);
    setIsModalOpen(true);
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setEditingEntry(null);
  };

  if (isLoading) {
    return <div className="loading">Loading dashboard...</div>;
  }

  return (
    <div>
      {/* Welcome Section */}
      <div className="welcome-section">
        <h2 className="welcome-title">Welcome back, {user?.displayName}! 👋</h2>
        <p className="welcome-subtitle">
          Here's an overview of your personal timeline.
        </p>
      </div>

      {/* Statistics Cards */}
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-title">Total Entries</div>
          <div className="stat-value">{stats?.totalEntries || 0}</div>
        </div>

        {stats?.entriesByType.map((item) => (
          <div key={item.entryType} className="stat-card">
            <div className="stat-title">{item.entryType}</div>
            <div className="stat-value">{item.count}</div>
          </div>
        ))}
      </div>

      {/* Recent Entries */}
      <div className="recent-entries-section">
        <div className="section-header">
          <h3 className="section-heading">Recent Entries</h3>
          <button
            onClick={() => setIsModalOpen(true)}
            className="btn btn-primary"
          >
            ➕ New Entry
          </button>
        </div>

        {recentEntries.length === 0 ? (
          <div className="empty-state">
            <div className="empty-state-icon">📝</div>
            <h3 className="empty-state-title">No Entries Yet</h3>
            <p className="empty-state-text">
              Start documenting your journey by creating your first timeline
              entry!
            </p>
            <button
              onClick={() => setIsModalOpen(true)}
              className="btn btn-primary btn-large"
            >
              Create Your First Entry
            </button>
          </div>
        ) : (
          <div>
            {recentEntries.map((entry) => (
              <TimelineEntry
                key={entry.id}
                entry={entry}
                onEdit={handleEditClick}
                onDelete={handleDeleteEntry}
              />
            ))}
          </div>
        )}
      </div>

      <EntryModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        title={
          editingEntry ? "Edit Timeline Entry" : "Create New Timeline Entry"
        }
      >
        <EntryForm
          entry={editingEntry}
          onSubmit={async (entry) => {
            if (editingEntry) {
              await handleUpdateEntry(entry as Partial<TimelineEntryType>);
            } else {
              await handleCreateEntry(entry as CreateTimelineEntry);
            }
          }}
          onCancel={handleModalClose}
          userId={user?.id || 1}
        />
      </EntryModal>
    </div>
  );
};
