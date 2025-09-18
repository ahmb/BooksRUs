import { useState } from "react";
import {
  useQuery,
  useMutation,
  useQueryClient,
  keepPreviousData,
} from "@tanstack/react-query";
import { fetchBooks, addToReadingList } from "@/lib/api";
import { BookCard } from "@/components/BookCard";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { type BookDto } from "@/lib/types";

const PAGE_SIZE = 12;

export default function Catalog() {
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const userId =
    (import.meta.env.VITE_DEFAULT_USER_ID as string) ?? "u1-demo";

  const qc = useQueryClient();

  const {
    data: books = [],
    isLoading,
    isError,
    error,
  } = useQuery<BookDto[]>({
    queryKey: ["books", { search, page, size: PAGE_SIZE }],
    queryFn: () => fetchBooks({ search, page, size: PAGE_SIZE }),
    placeholderData: keepPreviousData, // v5 way to keep previous page data
  });

  const mutation = useMutation({
    mutationFn: (bookId: string) => addToReadingList({ userId, bookId }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["reading-list", userId] });
    },
  });

  const canNext = books.length === PAGE_SIZE;

  return (
    <section>
      <div className="flex flex-col sm:flex-row sm:items-center gap-3 mb-6">
        <label htmlFor="catalog-search" className="sr-only">Search</label>
        <Input
          id="catalog-search"
          aria-label="Book Name"
          placeholder="Search by title or author..."
          value={search}
          onChange={(e) => {
            setSearch(e.target.value);
            setPage(1);
          }}
          className="sm:w-80"
        />
        <div className="ml-auto flex gap-2">
          <Button
            // Prev
            aria-label="Previous Button"
            variant="outline"
            onClick={() => setPage((p) => Math.max(1, p - 1))}
            disabled={page === 1}
            className="bg-black text-white hover:bg-black/80 disabled:bg-gray-300 disabled:text-white/70"
          >
            Prev
          </Button>

          <Button
            // Next
            variant="outline"
            aria-label="Next Button"
            onClick={() => setPage((p) => (canNext ? p + 1 : p))}
            disabled={canNext ?? false}
            className="bg-black text-white hover:bg-black/80 disabled:bg-gray-300 disabled:text-white/70"
          >
            Next
          </Button>
        </div>
      </div>

      {isLoading && <div className="text-gray-600">Loading...</div>}
      {isError && (
        <div className="text-red-600">
          Failed to load books{(error as Error)?.message ? `: ${(error as Error).message}` : "."}
        </div>
      )}

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {books.map((b) => (
          <BookCard
            key={b.id}
            book={b}
            onAdd={(id) => mutation.mutate(id)}
            adding={mutation.isPending}
          />
        ))}
      </div>

      {!isLoading && books.length === 0 && (
        <div className="text-gray-600 mt-6">No books found.</div>
      )}
    </section>
  );
}
