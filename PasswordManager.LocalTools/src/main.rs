
fn main() {
    let board = Board::new();

    
}

struct Board {
    cells:[[char;3]; 3]
}

impl Board {
    pub fn new() -> Self {
        return Self { cells: [['-', '-', '-'], ['-', '-', '-'], ['-', '-', '-']] };
    }
}

struct Cell {
    x:i8,
    y:i8,
    symbol:char
}

impl Cell {
    pub fn new(value_x: i8, value_y: i8) -> Self {
        return Self { x: value_x, y: value_y, symbol: '-' };
    }
}