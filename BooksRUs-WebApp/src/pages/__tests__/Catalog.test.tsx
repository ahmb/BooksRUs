import { describe, it, expect, vi, type Mock } from "vitest";
import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { renderWithProviders } from "@/test-utils";
import Catalog from "@/pages/Catalog";
import { type BookDto } from "@/lib/types";

vi.mock("@/lib/api", () => ({
  fetchBooks: vi.fn(),
  addToReadingList: vi.fn(),
}));
import { fetchBooks, addToReadingList } from "@/lib/api";

function makeBooks(count: number): BookDto[] {
  return Array.from({ length: count }).map((_, i) => ({
    id: `b${i + 1}`,
    isbn: String(1000000000 + i),
    title: `Book ${i + 1}`,
    author: `Author ${i + 1}`,
    year: 2000 + i,
    description: `Desc ${i + 1}`,
  }));
}

describe("Catalog", () => {
  it("shows books and handles pager state", async () => {
    const user = userEvent.setup();
    (fetchBooks as Mock).mockResolvedValueOnce(makeBooks(12));

    renderWithProviders(<Catalog />, { route: "/catalog" });

    // let isTrue = await screen.findByText(/Search/i);
    // expect(isTrue).toBeInTheDocument();
    const searchBox = await screen.findByRole("textbox", { name: /search/i });
    expect(searchBox).toBeInTheDocument();

    const prevBtn = screen.getByRole("button", { name: /prev/i });
    const nextBtn = screen.getByRole("button", { name: /next/i });
    expect(prevBtn).toBeDisabled();
    expect(nextBtn).toBeEnabled();

    (fetchBooks as Mock).mockResolvedValueOnce(makeBooks(3));
    await user.click(nextBtn);

    await waitFor(() => expect(fetchBooks).toHaveBeenCalledTimes(1));
    expect(nextBtn).toBeDisabled();
  });

  it("adds to reading list via mutation", async () => {
    (fetchBooks as Mock).mockResolvedValueOnce(makeBooks(1));
    (addToReadingList as Mock).mockResolvedValue({ id: "rl1" });

    const user = userEvent.setup();
    renderWithProviders(<Catalog />, { route: "/catalog" });

    const addBtns = await screen.findAllByRole("button", { name: /add to my list/i });
    await user.click(addBtns[0]);

    expect(addToReadingList).toHaveBeenCalledWith({
      userId: expect.any(String),
      bookId: "b1",
    });
  });
});
