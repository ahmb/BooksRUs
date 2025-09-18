import { BookOpen } from "lucide-react";

export default function Header() {
  return (
    <header className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-4"> {/* was py-6 */}
      <div className="flex items-center gap-3">
        <BookOpen className="w-8 h-8" />
        <h1 className="text-3xl md:text-4xl font-extrabold tracking-tight text-black">BooksRUs</h1>
      </div>
      <p className="mt-1 text-gray-600">Browse the catalog and build your reading list.</p>
    </header>
  );
}
