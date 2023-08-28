package main

import (
	"encoding/json"
	"fmt"
	"net"
	"time"
)

type PingData struct {
	Time      string
	LatencyMs int64
	Error     string
}

func main() {
	for i := 0; i < 100; i++ {
		var t = time.Now()
		conn, err := net.Dial("tcp", "ya.ru:80")
		errormessage := ""
		if err != nil {
			errormessage = err.Error()
		} else {
			conn.Close()
		}
		m := PingData{Time: time.Now().String(), LatencyMs: time.Since(t).Milliseconds(), Error: errormessage}
		b, jerr := json.Marshal(m)
		if jerr == nil {
			fmt.Printf("%s\n", b)
		}
		time.Sleep(1 * time.Second)
	}
}
