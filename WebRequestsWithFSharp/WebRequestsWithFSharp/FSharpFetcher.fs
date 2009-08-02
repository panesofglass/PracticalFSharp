open System
open System.IO
open System.Net

let tprintfn fmt =
  printf "[%d]" System.Threading.Thread.CurrentThread.ManagedThreadId
  printfn fmt

(* Synchronous *)
let internal executeWebRequest url =
  let request = WebRequest.Create(url: string)
  tprintfn "Created web request for %s." url
  
  let response = request.GetResponse()
  tprintfn "Getting response for %s." url
  
  use stream = response.GetResponseStream()    
  use reader = new StreamReader(stream)
  tprintfn "Reading the response stream from %s." url
  
  reader.ReadToEnd()

let ExecuteWebRequests urls =
  urls |> Seq.map executeWebRequest


(* Asynchronous *)
let internal asyncExecuteWebRequest url =
  async {
    let request = WebRequest.Create(url: string)
    tprintfn "Created web request for %s." url
  
    let! response = request.AsyncGetResponse()
    tprintfn "Getting response for %s." url
    
    use stream = response.GetResponseStream()    
    use reader = new StreamReader(stream)
    tprintfn "Reading the response stream from %s." url
    
    return! reader.AsyncReadToEnd()
  }

let AsyncExecuteWebRequests urls =
  urls
  |> Seq.map asyncExecuteWebRequest
  |> Async.Parallel
  |> Async.RunSynchronously
  |> Array.to_seq