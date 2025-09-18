import { useState } from "react";
import type { BookDto } from "@/lib/types";
import { Button } from "@/components/ui/button";
import BookDetailsDialog from "@/components/BookDetailsDialog";

export function BookCard({
  book,
  onAdd,
  adding,
}: {
  book: BookDto;
  onAdd?: (id: string) => void;
  adding?: boolean;
}) {
  const [open, setOpen] = useState(false);

  return (
    <article className="rounded-2xl border border-gray-200 p-4 hover:shadow-sm transition">
      {/* Only minimal info on the card */}
      <div className="mt-1 text-xl font-bold">{book.title}</div>
      <div className="text-gray-600">
        {book.author} · {book.year}
      </div>

      <div className="mt-4 flex gap-2">
        {onAdd && (
          <Button
            disabled={adding}
            className="bg-green-600 hover:bg-green-700 hover:text-blue-500 text-white"
            onClick={() => onAdd(book.id)}
          >
            {adding ? "Adding..." : "Add to My List"}
          </Button>
        )}
        <Button
          variant="outline"
          onClick={() => setOpen(true)}
          className="text-white hover:text-blue-500"
          aria-label={`View details for ${book.title}`}
        >
          Details
        </Button>
      </div>

      {/* Modal */}
      <BookDetailsDialog
        bookId={book.id}
        open={open}
        onOpenChange={setOpen}
        initialData={book}
      />
    </article>
  );
}
