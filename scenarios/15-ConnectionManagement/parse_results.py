import sys
import json

try:
    # Read from stdin if file path not provided, else read file
    if len(sys.argv) > 1:
        with open(sys.argv[1], 'r') as f:
            data = json.load(f)
    else:
        data = json.load(sys.stdin)

    # Helper to safely get nested keys
    def get_metric(metric_name, value_key):
        try:
            # Check if 'values' key exists (older k6 or specific output format)
            if "values" in data["metrics"][metric_name]:
                return data["metrics"][metric_name]["values"][value_key]
            # Direct access (newer k6 summary export)
            else:
                return data["metrics"][metric_name][value_key]
        except (KeyError, TypeError):
            return "N/A"

    avg = get_metric("http_req_duration", "avg")
    p50 = get_metric("http_req_duration", "p(50)")
    p95 = get_metric("http_req_duration", "p(95)")
    p99 = get_metric("http_req_duration", "p(99)")
    rps = get_metric("http_reqs", "rate")

    # Format numbers to 2 decimal places if they are floats
    def fmt(val):
        if isinstance(val, (int, float)):
            return "{:.2f}".format(val)
        return val

    print(f"{fmt(avg)},{fmt(p50)},{fmt(p95)},{fmt(p99)},{fmt(rps)}")

except Exception as e:
    # Print zeros on failure to not break the csv structure
    print("0,0,0,0,0")
