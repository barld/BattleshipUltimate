using System.Collections.Immutable;

namespace BattleshipDotNet.Logic;

public record Game
{
    public required PlayerBoard? Board1 { get; init; }
    public required PlayerBoard? Board2 { get; init; }
}

public record Board
{
    public required string ClientId { get; init; }
    public required string PlayerName { get; init; }
    public required ImmutableList<Hit> Hits { get; init; }
}

public record PlayerBoard : Board
{
    public required ImmutableList<Ship> Ships { get; init; }
}

public sealed record Hit
{
    public required Coordinate Coordinate { get; init; }
    public required bool Strike { get; init; }
}

public record Coordinate
{
    public required char X { get; init; }
    public required int Y { get; init; }
}

public record Ship
{
    public required ShipKind Kind { get; init; }
    public required ImmutableList<Coordinate> Coordinates { get; init; }
}

public enum ShipKind
{
    Carrier,
    Battleship,
    Submarine,
    PatrolBoat
}

public static class CoordinateMethods
{
    public static bool IsInline(this IEnumerable<Coordinate> coordinates)
    {
        return
            (coordinates.Select(c => c.X).Distinct().Count() == 1
                && coordinates.Select(c => c.Y).Order().Pairwise().All(p => p.Item1 + 1 == p.Item2))
            ||
            (coordinates.Select(c => c.Y).Distinct().Count() == 1
                && coordinates.Select(c => c.X).Order().Pairwise().All(p => p.Item1 + 1 == p.Item2));
    }

    public static bool IsOnBoard(this Coordinate coordinate)
    {
        return coordinate.X >= 'A' && coordinate.X <= 'J' && coordinate.Y >= 1 && coordinate.Y <= 10;
    }

    public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> source)
    {
        var previous = default(T);
        using (var it = source.GetEnumerator())
        {
            if (it.MoveNext())
                previous = it.Current;

            while (it.MoveNext())
                yield return (previous, previous = it.Current);
        }
    }
}

public static class ShipKindMethods
{
    public static int Size(this ShipKind kind) => kind switch
    {
        ShipKind.Carrier => 6,
        ShipKind.Battleship => 4,
        ShipKind.Submarine => 3,
        ShipKind.PatrolBoat => 2,
        _ => throw new NotImplementedException(),
    };

    public static int Amount(this ShipKind kind) => kind switch
    {
        ShipKind.Carrier => 1,
        ShipKind.Battleship => 2,
        ShipKind.Submarine => 3,
        ShipKind.PatrolBoat => 4,
        _ => throw new NotImplementedException(),
    };

    public static int TotalAmountOfShips
        => Enum.GetValues(typeof(ShipKind))
        .Cast<ShipKind>()
        .Sum(kind => kind.Amount());
            
}

public static class PlayerBoardValidator
{
    public static bool IsValid(this PlayerBoard board)
    {
        return HasRightAmountOfShips(board)
            && board.Ships.All(IsShipValidPlacedOnBoard)
            && HasNoCollidingShips(board);
    }

    private static bool HasNoCollidingShips(PlayerBoard board)
    {
        return board.Ships.SelectMany(ship => ship.Coordinates).Distinct().Count() == board.Ships.SelectMany(ship => ship.Coordinates).Count();
    }

    private static bool HasRightAmountOfShips(PlayerBoard board)
    {
        return board.Ships.Count == ShipKindMethods.TotalAmountOfShips
            && board.Ships.GroupBy(ship => ship.Kind)
                .All(group => group.Key.Amount() == group.Count());
    }

    private static bool IsShipValidPlacedOnBoard(Ship ship)
    {
        return ship.Kind.Amount() == ship.Coordinates.Count
            && ship.Coordinates.IsInline()
            && ship.Coordinates.All(CoordinateMethods.IsOnBoard);
    }
}
