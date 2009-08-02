open TraceBuilder

let traceDemo =
  trace {
    // A normal let expression that will not be bound.
    let x = 1
    
    // A let using the bind expression
    let! y = 2
    
    let sum = x + y
    
    // Returns
    return sum
  }

// Execute the trace.
let result = traceDemo ()