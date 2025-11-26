import { Header } from "../components/layout/Header";
import { TimelineView } from "../components/timeline/TimelineView";

export const TimelinePage = () => {
  return (
    <div>
      <Header
        title="My Timeline"
        subtitle="View and manage all your timeline entries"
      />
      <TimelineView />
    </div>
  );
};
