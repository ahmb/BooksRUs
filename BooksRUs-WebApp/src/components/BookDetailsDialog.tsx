import { useQuery } from "@tanstack/react-query";
import { X } from "lucide-react";
import { fetchBook } from "@/lib/api";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
  DialogFooter,
  DialogClose
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import type { BookDto } from "@/lib/types";

type Props = {
  bookId: string;
  open: boolean;
  onOpenChange: (open: boolean) => void;
  /** Optional data to show immediately while we fetch latest details */
  initialData?: BookDto;
};

export default function BookDetailsDialog({
  bookId,
  open,
  onOpenChange,
  initialData,
}: Props) {
  const { data, isLoading, isError } = useQuery<BookDto>({
    queryKey: ["book", bookId],
    queryFn: () => fetchBook(bookId),
    enabled: open,
    initialData,
  });

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-lg">
            <DialogClose
      className="
        absolute right-4 top-4
        rounded-md bg-black/60 text-white
        opacity-100 hover:bg-black hover:text-white
        focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring
      "
    >
      <X className="h-4 w-4" />
      <span className="sr-only">Close</span>
    </DialogClose>
        <DialogHeader>
          <DialogTitle>{data?.title ?? "Book Details"}</DialogTitle>
          <DialogDescription>
            {isLoading
              ? "Loading book details…"
              : isError
              ? "Failed to load book details."
              : `${data?.author} · ${data?.year}`}
          </DialogDescription>
        </DialogHeader>

        {!isLoading && !isError && data && (
          <div className="space-y-2 text-sm">
            <Detail label="Id" value={data.id} />
            <Detail label="Title" value={data.title} />
            <Detail label="Author" value={data.author} />
            <Detail label="Year" value={String(data.year)} />
            <Detail label="ISBN" value={data.isbn} />
            {data.description && (
              <div>
                <div className="font-medium text-gray-800">Description</div>
                <p className="text-gray-700 mt-1">{data.description}</p>
              </div>
            )}
          </div>
        )}

        <DialogFooter className="mt-4">
          <Button className="bg-white text-white"  onClick={() => onOpenChange(false)}>Close</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function Detail({ label, value }: { label: string; value: string }) {
  return (
    <div className="flex items-start gap-2">
      <div className="w-24 shrink-0 text-gray-600">{label}</div>
      <div className="text-gray-900">{value}</div>
    </div>
  );
}
