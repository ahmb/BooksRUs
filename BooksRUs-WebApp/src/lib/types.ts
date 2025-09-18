export type BookDto = { id: string; isbn: string; title: string; author: string; year: number; description?: string | null; };
export type CreateBookRequest = { isbn: string; title: string; author: string; year: number; description?: string | null; };
export type ReadingListItemDto = { id: string; userId: string; bookId: string; addedAt: string; };
export type AddReadingListRequest = { userId: string; bookId: string; };
