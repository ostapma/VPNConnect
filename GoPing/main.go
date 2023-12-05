package main

import (
	"bufio"
	"encoding/json"
	"fmt"
	"net"
	"os"
	"time"
)

type PingData struct {
	Ip        string
	Time      string
	LatencyMs int64
	Error     string
}

const PingIntervalMs = 1000
const Port = 80

func main() {

	if len(os.Args) < 2 {
		PrintError("Ping target address is not specified")
		return
	}
	var stop = false

	go func() {
		scanner := bufio.NewScanner(os.Stdin)
		for scanner.Scan() {
			if scanner.Text() == "s" {
				stop = true
				break
			}
		}
	}()
	var latencyMs int64 = 0
	var ip net.IP = net.ParseIP(os.Args[1])

	if ip == nil {
		ips, iperr := net.LookupIP(os.Args[1])
		if iperr != nil {
			PrintError(iperr.Error())
		} else {
			ip = ips[0]
		}
	}

	if ip != nil {
		for !stop {
			var tstartping = time.Now()
			d := net.Dialer{Timeout: time.Second * 10}
			pingError := ""
			conn, err := d.Dial("tcp4", fmt.Sprintf("%s:%d", ip, Port))
			if err != nil {
				pingError = err.Error()
			} else {
				conn.Close()

				latencyMs = time.Since(tstartping).Milliseconds()
			}
			PrintPing(ip.String(), tstartping.String(), latencyMs, pingError)
			time.Sleep(PingIntervalMs * time.Millisecond)
		}
	} else {
		PrintError("Invalid ping target address")
	}

}

func PrintPing(ip string, startTime string, latencyMs int64, errormessage string) {
	m := PingData{Ip: ip, Time: startTime, LatencyMs: latencyMs, Error: errormessage}
	b, jerr := json.Marshal(m)
	if jerr == nil {
		fmt.Printf("%s\n", b)
	} else {
		PrintError(jerr.Error())
	}
}

func PrintError(errormessage string) {
	fmt.Printf("%s\n", errormessage)
}
