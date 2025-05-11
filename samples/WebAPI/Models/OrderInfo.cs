// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace WebAPI.Models;

public record OrderInfo
{
    public OrderInfo(string Item, int Quantity, double Price)
    {
        this.Item = Item;
        this.Quantity = Quantity;
        this.Price = Price;
    }

    public string Item { get; init; }
    public int Quantity { get; init; }
    public double Price { get; init; }

    public void Deconstruct(out string Item, out int Quantity, out double Price)
    {
        Item = this.Item;
        Quantity = this.Quantity;
        Price = this.Price;
    }
}
