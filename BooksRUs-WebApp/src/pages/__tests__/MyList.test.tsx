import { describe, it, expect, vi, type Mock } from "vitest";
import { screen } from "@testing-library/react";
import { renderWithProviders } from "@/test-utils";
import MyList from "@/pages/MyList";
import { type ReadingListItemDto } from "@/lib/types";

vi.mock("@/lib/api", () => ({
  fetchReadingList: vi.fn(),
}));
import { fetchReadingList } from "@/lib/api";

const items: ReadingListItemDto[] = [
  { id: "i1", userId: "u1-demo", bookId: "b1", addedAt: new Date().toISOString() },
  { id: "i2", userId: "u1-demo", bookId: "b2", addedAt: new Date().toISOString() },
];

describe("MyList", () => {
  it("renders reading list items", async () => {
    (fetchReadingList as Mock).mockResolvedValueOnce(items);

    renderWithProviders(<MyList />, { route: "/my-list" });

    expect(await screen.findByText(/book id: b1/i)).toBeInTheDocument();
    expect(screen.getByText(/book id: b2/i)).toBeInTheDocument();
  });
});
