import axios from "axios";
import type {
  BookDto,
  AddReadingListRequest,
  ReadingListItemDto,
} from "./types";

const api = axios.create({
  baseURL:
    import.meta.env.VITE_API_BASE_URL !== undefined
      ? (import.meta.env.VITE_API_BASE_URL as string)
      : "/api",
  headers: { "Content-Type": "application/json" },
});

export async function fetchBooks(params: {
  search?: string;
  page?: number;
  size?: number;
}): Promise<BookDto[]> {
  const { search = "", page = 1, size = 20 } = params;
  const res = await api.get<BookDto[]>("/api/books", {
    params: { search, page, size },
  });
  return res.data;
}

export async function fetchBook(id: string): Promise<BookDto> {
  const res = await api.get<BookDto>(`/api/books/${id}`);
  return res.data;
}

export async function addToReadingList(
  req: AddReadingListRequest
): Promise<ReadingListItemDto> {
  const res = await api.post<ReadingListItemDto>("/api/reading-list", req);
  return res.data;
}

export async function fetchReadingList(
  userId: string
): Promise<ReadingListItemDto[]> {
  const res = await api.get<ReadingListItemDto[]>("/api/reading-list", {
    params: { userId },
  });
  return res.data;
}