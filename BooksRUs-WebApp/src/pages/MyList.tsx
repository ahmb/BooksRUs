import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { fetchReadingList } from "@/lib/api";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

export default function MyList() {
  const [userId, setUserId] = useState(
    localStorage.getItem("booksrus:userId") || (import.meta.env.VITE_DEFAULT_USER_ID as string) || "u1-demo"
  );

  useEffect(() => { localStorage.setItem("booksrus:userId", userId); }, [userId]);

  const { data: items, isLoading, isError, refetch, isFetching } = useQuery({
    queryKey: ["reading-list", userId],
    queryFn: () => fetchReadingList(userId),
    enabled: !!userId,
  });

  return (
    <section>
      <div className="flex flex-col sm:flex-row sm:items-center gap-3 mb-6">
        <div className="flex items-center gap-2">
          <label className="text-sm font-medium">User ID</label>
          <Input value={userId} onChange={(e) => setUserId(e.target.value)} className="sm:w-72" />
        </div>
        <Button className="bg-green-600 hover:bg-green-700 text-white" onClick={() => refetch()} disabled={isFetching} aria-label="Refresh List">
          {isFetching ? "Refreshing..." : "Refresh"}
        </Button>
      </div>

      {isLoading && <div className="text-gray-600">Loading...</div>}
      {isError && <div className="text-red-600">Failed to load reading list.</div>}

      <ul className="space-y-3">
        {items?.map((x) => (
          <li key={x.id} className="rounded-2xl border border-gray-200 p-4">
            <div className="text-sm text-gray-600">Added: {new Date(x.addedAt).toLocaleString()}</div>
            <div className="font-semibold">Book Id: {x.bookId}</div>
          </li>
        ))}
      </ul>

      {!isLoading && items?.length === 0 && <div className="text-gray-600 mt-6">Your reading list is empty.</div>}
    </section>
  );
}
