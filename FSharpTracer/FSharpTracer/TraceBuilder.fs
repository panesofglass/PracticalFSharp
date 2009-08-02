open System.Diagnostics

// Builder class for the trace workflow
type TraceBuilder () =
  let sw = new Stopwatch()
  member x.Bind (value, transform) =
    printfn "%A\tBinding %A." sw.ElapsedMilliseconds value
    transform value
  member x.Return (value) =
    printfn "%A\tReturning result: %A." sw.ElapsedMilliseconds value
    sw.Stop()
    fun () -> value
  member x.Delay (transform) =
    printfn "Starting traced execution ..."
    sw.Start()
    fun () -> transform ()

let trace = new TraceBuilder()