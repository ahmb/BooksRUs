import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { renderWithProviders } from "@/test-utils";
import { BookCard } from "@/components/BookCard";
import { type BookDto } from "@/lib/types";
import { describe, it, expect, vi } from "vitest";

const book: BookDto = {
  id: "b1",
  isbn: "1111111111",
  title: "The Odyssey",
  author: "Homer",
  year: 1996,
  description: "Epic poem",
};

it("renders book info and calls onAdd", async () => {
  const user = userEvent.setup();
  const onAdd = vi.fn();

  renderWithProviders(<BookCard book={book} onAdd={onAdd} adding={false} />);

  expect(screen.getByText(/the odyssey/i)).toBeInTheDocument();
  expect(screen.getByText(/homer/i)).toBeInTheDocument();
  expect(screen.getByRole("button", { name: /add to my list/i })).toBeEnabled();

  await user.click(screen.getByRole("button", { name: /add to my list/i }));
  expect(onAdd).toHaveBeenCalledWith("b1");
});
