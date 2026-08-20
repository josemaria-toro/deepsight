DROP SCHEMA deepsight CASCADE;

CREATE SCHEMA deepsight AUTHORIZATION postgres;

CREATE TABLE deepsight.dependencies
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_bta_data_input bytea,
    c_bta_data_output bytea,
    c_dbl_duration double precision NOT NULL,
    c_str_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_bln_success boolean NOT NULL,
    c_str_target character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_type character varying(32) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_dependencies PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.dependencies OWNER to postgres;

CREATE TABLE deepsight.errors
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_str_category character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_message character varying(4096) COLLATE pg_catalog."default" NOT NULL,
    c_str_severity character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_stack_trace character varying(4096) COLLATE pg_catalog."default" NOT NULL,
    c_str_error_type character varying(128) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_errors PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.errors OWNER to postgres;

CREATE TABLE deepsight.events
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_str_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_events PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.events OWNER to postgres;

CREATE TABLE deepsight.metrics
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_str_dimension character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_dbl_value double precision NOT NULL,
    CONSTRAINT pk_metrics PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.metrics OWNER to postgres;

CREATE TABLE deepsight.pageviews
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_str_device_type character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_url character varying(1024) COLLATE pg_catalog."default",
    c_str_user_agent character varying(1024) COLLATE pg_catalog."default",
    CONSTRAINT pk_pageviews PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.pageviews OWNER to postgres;

CREATE TABLE deepsight.requests
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_bta_data_input bytea,
    c_bta_data_output bytea,
    c_dbl_duration double precision NOT NULL,
    c_str_endpoint character varying(1024) COLLATE pg_catalog."default" NOT NULL,
    c_str_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_int_status_code integer NOT NULL,
    c_bln_success boolean NOT NULL,
    c_str_type character varying(16) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_requests PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.requests OWNER to postgres;

CREATE TABLE deepsight.tests
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_dbl_duration double precision NOT NULL,
    c_str_message character varying(4096) COLLATE pg_catalog."default" NOT NULL,
    c_str_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_bln_success boolean NOT NULL,
    CONSTRAINT pk_tests PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.tests OWNER to postgres;

CREATE TABLE deepsight.traces
(
    c_uid_id uuid NOT NULL,
    c_uid_tenant_id uuid NOT NULL,
    c_tsp_timestamp timestamp with time zone NOT NULL,
    c_tsp_created_at timestamp with time zone NOT NULL,
    c_tsp_updated_at timestamp with time zone NOT NULL,
    c_str_trace_id character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_span_id character varying(16) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_app_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_ip_address character varying(64) COLLATE pg_catalog."default" NOT NULL,
    c_str_client_version character varying(32) COLLATE pg_catalog."default" NOT NULL,
    c_str_host_name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_jsn_metadata jsonb,
    c_str_category character varying(128) COLLATE pg_catalog."default" NOT NULL,
    c_str_message character varying(4096) COLLATE pg_catalog."default" NOT NULL,
    c_str_severity character varying(16) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_traces PRIMARY KEY (c_uid_id)
) TABLESPACE pg_default;

ALTER TABLE deepsight.traces OWNER to postgres;
