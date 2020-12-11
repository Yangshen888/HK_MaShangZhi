<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.mCheck.query.totalCount"
        :pageSize="stores.mCheck.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <!-- <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.mCheck.query.kw"
                      placeholder="请输入登记人员"
                      @on-search="handleSearchDispatch()"
                    ></Input>-->
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                  >添加</Button
                >
                <!-- <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportCuisine"
                  title="导入"
                >导入</Button>
                <Button
                  v-can="'export'"
                  icon="ios-cloud-download-outline"
                  type="primary"
                  @click="handleExportCuisine('export')"
                  title="导出"
                >导出</Button>
                <Button
                  v-can="'yjexport'"
                  icon="ios-cloud-download-outline"
                  type="primary"
                  @click="handleExportCuisineyj('export')"
                  title="一键导出"
                >一键导出</Button>-->
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.mCheck.data"
          :columns="stores.mCheck.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  type="error"
                  v-can="'deletes'"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                ></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="晨检人员详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'infolist'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-contacts"
                @click="handleShowInfoList(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="应检人数" prop="shouldCount">
            <InputNumber
              v-model="formModel.fields.shouldCount"
              :min="0"
              :max="500"
              :precision="0"
            ></InputNumber>
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="检查员" prop="inspector">
            <Input v-model="formModel.fields.inspector" placeholder="检查员" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="部门" prop="departmentId">
            <Select filterable v-model="formModel.fields.departmentId">
              <Option
                v-for="item in departments"
                :value="item.id"
                :key="item.id"
                >{{ item.name }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="采购日期" prop="purchaseDate">
            <DatePicker
              v-model="formModel.fields.purchaseDate"
              @on-change="formModel.fields.purchaseDate=$event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="采购日期"
              style="width: 150px"
              :options="options3"
            ></DatePicker>
          </FormItem>
        </Row>
        
        <Row :gutter="16">
          <FormItem label="供应商" prop="supplier">
            <Input v-model="formModel.fields.supplier" placeholder="供应商" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="note">
            <Input v-model="formModel.fields.note" type="textarea" placeholder="备注" />
          </FormItem>
        </Row> -->
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>

    <Drawer
      title="详情"
      v-model="formModel1.opened"
      width="600"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModel1.fields"
        ref="formdispatch22"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="组织" prop="organizationName">
            <Input
              v-model="formModel1.fields.name"
              placeholder="组织"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="时间" prop="createdAt">
            <Input
              v-model="formModel1.fields.createdAt"
              placeholder="时间"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="检查员" prop="inspector">
            <Input
              v-model="formModel1.fields.inspector"
              placeholder="检查员"
              :readonly="true"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="应检人数" prop="shouldCount">
            <Input
              v-model="formModel1.fields.shouldCount"
              placeholder="应检人数"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="实际人数" prop="actualCount">
            <Input
              v-model="formModel1.fields.actualCount"
              placeholder="实际人数"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="不合格数" prop="unqualifiedCount">
            <Input
              v-model="formModel1.fields.unqualifiedCount"
              placeholder="不合格数"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="备注" prop="note">
            <Input v-model="formModel1.fields.note" type="textarea" placeholder="备注" />
          </FormItem>
        </Row>-->
        <!-- <Row :gutter="16">
          <FormItem label="采购票证" prop="ticketimgs">
            <Input v-model="formModel1.fields.ticketimgs" placeholder="采购票证" />
          </FormItem>
        </Row>-->
      </Form>
      <div class="demo-drawer-footer">
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel1.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
    <!-- <Drawer
      title="餐饮信息导入"
      v-model="formimport.opened"
      width="600"
      :mask-closable="true"
      :mask="true"
    >
      <div>
        <input
          ref="inputer"
          id="upload"
          type="file"
          @change="importfxx"
          accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        />
        <Button
          icon="ios-cloud-download"
          type="warning"
          @click="handleimportmodel"
          title="餐饮信息导入模板下载"
        >餐饮信息导入模板下载</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button>

        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>-->
    <Drawer
      title="人员晨检详情列表"
      v-model="formInfo.opened"
      width="900"
      :mask-closable="true"
      :mask="true"
    >
      <Card>
        <dz-table
          :totalCount="stores.mCheckInfo.query.totalCount"
          :pageSize="stores.mCheckInfo.query.pageSize"
          @on-page-change="handlePageChanged2"
          @on-page-size-change="handlePageSizeChanged2"
        >
          <div slot="searcher">
            <section class="dnc-toolbar-wrap">
              <Row :gutter="16">
                <Col span="16">
                  <Form inline @submit.native.prevent>
                    <FormItem>
                      <!-- <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.mCheck.query.kw"
                      placeholder="请输入登记人员"
                      @on-search="handleSearchDispatch()"
                      ></Input>-->
                    </FormItem>
                  </Form>
                </Col>
                <Col span="8" class="dnc-toolbar-btns">
                  <ButtonGroup class="mr3">
                    <Button
                      
                      class="txt-danger"
                      icon="md-trash"
                      title="删除"
                      @click="handleBatchCommand2('delete')"
                    ></Button>
                    <Button
                      icon="md-refresh"
                      title="刷新"
                      @click="handleRefresh2"
                    ></Button>
                  </ButtonGroup>
                  <Button
                    icon="md-create"
                    type="primary"
                    @click="handleShowCreateWindow2"
                    title="添加"
                    >添加</Button
                  >
                </Col>
              </Row>
            </section>
          </div>
          <Table
            slot="table"
            ref="tables"
            :border="false"
            size="small"
            :highlight-row="true"
            :data="stores.mCheckInfo.data"
            :columns="stores.mCheckInfo.columns"
            @on-select="handleSelect2"
            @on-selection-change="handleSelectionChange2"
            @on-refresh="handleRefresh2"
            :row-class-name="rowClsRender2"
            @on-page-change="handlePageChanged2"
            @on-page-size-change="handlePageSizeChanged2"
          >
            <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
            </template>-->
            <template slot-scope="{ row, index }" slot="action2">
              <!-- <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete2(row)">
                <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                  <Button
                    type="error"
                    v-can="'deletes'"
                    size="small"
                    shape="circle"
                    icon="md-trash"
                  ></Button>
                </Tooltip>
              </Poptip>-->
              <Tooltip
                placement="top"
                content="编辑"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  type="primary"
                  size="small"
                  shape="circle"
                  icon="md-create"
                  @click="handleEdit2(row)"
                ></Button>
              </Tooltip>
              <Tooltip
                placement="top"
                content="详情"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  type="warning"
                  size="small"
                  shape="circle"
                  icon="md-search"
                  @click="handleDetail2(row)"
                ></Button>
              </Tooltip>
            </template>
          </Table>
        </dz-table>
      </Card>
    </Drawer>
    <Drawer
      :title="formTitle2"
      v-model="formModel2.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel2.fields"
        ref="form2"
        :rules="formModel2.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="受检人员" prop="userId">
            <Select filterable v-model="formModel2.fields.userId" :disabled="this.formModel2.mode=='show'" >
              <Option
                v-for="item in userlist"
                :value="item.sysUserId"
                :key="item.sysUserId"
                >{{ item.name }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="应检人数" prop="shouldCount">
            <InputNumber
              v-model="formModel.fields.shouldCount"
              :min="0"
              :max="500"
              :precision="0"
            ></InputNumber>
          </FormItem>
        </Row> -->

        <Row :gutter="16">
          <FormItem label="职位" prop="duty">
            <Input v-model="formModel2.fields.duty" placeholder="职位" :readonly="this.formModel2.mode=='show'" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="证件号" prop="cardNumber">
            <Input
              v-model="formModel2.fields.cardNumber"
              placeholder="证件号"
              :readonly="this.formModel2.mode=='show'"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="考勤" prop="attendance">
            <Select filterable v-model="formModel2.fields.attendance" :disabled="this.formModel2.mode=='show'" >
              <Option
                v-for="item in stores.mCheckInfo.sources.attendance"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="时长" prop="attendanceTime">
            <Select filterable v-model="formModel2.fields.attendanceTime" :disabled="this.formModel2.mode=='show'" >
              <Option
                v-for="item in stores.mCheckInfo.sources.attendance_time"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="晨检状态" prop="checkTypes">
            <Select multiple filterable v-model="checkTypes" :disabled="this.formModel2.mode=='show'" >
              <Option
                v-for="item in typelist"
                :value="item.typeId"
                :key="item.typeId"
                >{{ item.keyValue }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="体温" prop="temperature">
            <Input v-model="formModel2.fields.temperature" placeholder="体温" :readonly="this.formModel2.mode=='show'" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="处理意见" prop="prosessInfo">
            <Select filterable v-model="formModel2.fields.prosessInfo" :disabled="this.formModel2.mode=='show'" >
              <Option
                v-for="item in stores.mCheckInfo.sources.prosess_info"
                :value="item.value"
                :key="item.value"
                >{{ item.label }}</Option
              >
            </Select>
          </FormItem>
        </Row>

        <!-- <Row :gutter="16">
          <FormItem label="采购日期" prop="purchaseDate">
            <DatePicker
              v-model="formModel.fields.purchaseDate"
              @on-change="formModel.fields.purchaseDate=$event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="采购日期"
              style="width: 150px"
              :options="options3"
            ></DatePicker>
          </FormItem>
        </Row>
        
        <Row :gutter="16">
          <FormItem label="供应商" prop="supplier">
            <Input v-model="formModel.fields.supplier" placeholder="供应商" />
          </FormItem>
        </Row> -->
        <Row :gutter="16">
          <FormItem label="情况说明" prop="note">
            <Input
              v-model="formModel2.fields.note"
              type="textarea"
              placeholder="情况说明"
              :readonly="this.formModel2.mode=='show'"
            />
          </FormItem>
        </Row>
        <Row v-show="!checkShow()">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon
                  type="ios-eye-outline"
                  style="float: left;"
                  @click.native="handleView(item.url)"
                ></Icon>
                <Icon
                  type="ios-trash-outline"
                  style="float: right;"
                  @click.native="handleRemove(item.name)"
                ></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg','jpeg','png']"
            :max-size="5120"
            :data="{scene:'晨检图片',groupType:'morningCheck'}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            type="drag"
            :action="actionurl"
            :headers="postheaders"
            style="display: inline-block;width:58px;"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
        <Row v-if="checkShow()">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
        </Row>
      </Form>
      <div class="demo-drawer-footer" v-if="this.formModel2.mode!='show'">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable2"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel2.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  GetList, //显示列表
  GetInfoList,
  Create, //新增
  Load, //获取选定信息
  Edit, //编辑
  batchCommand, //批量删除
  Delete, //单个删除
  // GetImport, //导入
  // GetExport //导出
  GetDepartments,
  CreateInfo,
  LoadInfo,
  EditInfo,
  batchCommand2,
  TypeList,
  UsersList,
} from "@/api/PurchasesInfo/MorningCheck";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "morningCheck",
  components: {
    DzTable,
  },
  data() {
    const globalvalidateuser = (rule, value, callback) => {
      if (value == 0 ) {
        
          callback(new Error("请选择受检人员"));
        
      }
      callback();
    };
    return {
      scene:"",
      groupType:"",
      options3: {
        disabledDate(date) {
          return date && date.valueOf() > Date.now();
        },
      },

      uploadList: [],
      defaultList: [],
      actionurl: "",
      postheaders: "",
      imgName: "",
      loadingStatus: false,
      updisabled: false,
      visible: false,

      //导入
      url: config.baseUrl.dev,
      importdisable: false,

      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false,
      },

      
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        Import: { name: "Import", title: "导入" },
        export: { name: "export", title: "导出" },
      },
      inglist: [],
      model1: [],
      model2: [],
      departments: [],
      typelist: [],
      checkTypes: [],
      userlist: [],
      o_inspection_id: 0,
      inspection_id: 0,
      stores: {
        mCheck: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            kw2: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            sexList: [
              {
                value: "正常营业",
                label: "正常营业",
              },
              {
                value: "暂停营业",
                label: "暂停营业",
              },
            ],
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "id" },
            {
              title: "组织",
              key: "schoolName",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "应检人数",
              key: "shouldCount",
              minWidth: 150,
              sortable: true,
            },
            {
              title: "实际人数",
              key: "actualCount",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "不合格数",
              key: "unqualifiedCount",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "检查员",
              key: "inspector",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "时间",
              key: "createdAt",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "操作",
              align: "createdDate",
              fixed: "right",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
        mCheckInfo: {
          data: [],
          columns: [
            { type: "selection", minwidth: 50, key: "id" },
            {
              title: "时间",
              key: "createdAt",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "姓名",
              key: "userName",
              minWidth: 150,
              sortable: true,
            },
            {
              title: "职位",
              key: "duty",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "状态",
              key: "checkStatus",
              minWidth: 120,
              sortable: true,
            },
            {
              title: "操作",
              align: "createdDate",
              fixed: "right",
              width: 100,
              className: "table-command-column",
              slot: "action2",
            },
          ],
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            inspectionid: 0,
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            attendance: [
              {
                value: "0",
                label: "到岗",
              },
              {
                value: "1",
                label: "事假",
              },
              {
                value: "2",
                label: "病假",
              },
              {
                value: "3",
                label: "其他",
              },
            ],
            attendance_time: [
              {
                value: "0",
                label: "全天",
              },
              {
                value: "1",
                label: "上午",
              },
              {
                value: "2",
                label: "下午",
              },
            ],
            prosess_info: [
              {
                value: "0",
                label: "正常上岗",
              },
              {
                value: "1",
                label: "离岗休息",
              },
              {
                value: "2",
                label: "整改到岗",
              },
            ],
          },
        },
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          organizationId: "",
          organizationName: "",
          inspector: "",
          shouldCount: 0,
          unqualifiedCount: 0,
          actualCount: 0,
          id: 0,
          type: "",
          supplier: "",
          note: "",
          createdAt: "",
          name: "",
          departmentId: 0,
        },
        rules: {
          register: [
            {  type: "string", required: true, message: "请输入登记人员" },
          ],
          registerDate: [{ required: true, message: "请选择登记日期" }],
          purchaseUser: [
            { type: "string", required: true, message: "请输入采购人员" },
          ],
          purchaseDate: [{ required: true, message: "请选择采购日期" }],
          type: [{ type: "string", required: true, message: "请输入品种" }],
        },
      },
      formModel1: {
        opened: false,
        selection: [],
        fields: {
          register: "",
          registerDate: "",
          purchaseUser: "",
          purchaseDate: "",
          id: "",
          type: "",
          supplier: "",
          note: "",
          inspector: "",
        },
      },
      formInfo: {
        opened: false,
        selection: [],
        fields: {
          register: "",
          registerDate: "",
          purchaseUser: "",
          purchaseDate: "",
          id: "",
          type: "",
          supplier: "",
          note: "",
        },
      },
      formModel2: {
        mode: "create",
        opened: false,
        selection: [],

        fields: {
          attendance: "",
          attendanceTime: "",
          cardNumber: "",
          checkStatus: "",
          createdAt: "",
          duty: "",
          id: 0,
          imgUrl: "",
          inspectionId: 0,
          inspectionInformationId: 0,
          note: "",
          organizationId: 0,
          ownerInspectionId: 0,
          peopleStatus: "",
          peopleStatusName: "",
          prosessInfo: "",
          temperature: "",
          userId: 0,
          userName: "",
        },
        rules: {
          // register: [
          //   { type: "string", required: true, message: "请输入登记人员" },
          // ],
          // registerDate: [{ required: true, message: "请选择登记日期" }],
          // purchaseUser: [
          //   { type: "string", required: true, message: "请输入采购人员" },
          // ],
          userId: [{validator: globalvalidateuser, required: true, message: "请选择受检人员" }],
          attendance: [{ required: true, message: "请选择考勤" }],
          attendanceTime: [{ required: true, message: "请选择时长" }],
          prosessInfo: [{ required: true, message: "请选择处理意见" }],
          // type: [{ type: "string", required: true, message: "请输入品种" }],
        },
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑信息";
      }
      return "";
    },
    formTitle2() {
      if (this.formModel2.mode === "create") {
        return "新增";
      }
      if (this.formModel2.mode === "edit") {
        return "编辑";
      }
      if(this.formModel2.mode==="show"){
        return "详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRows2() {
      return this.formInfo.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.id);
    }, //删除
    selectedRowsId2() {
      return this.formInfo.selection.map((x) => x.id);
    }, //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
      GetList(this.stores.mCheck.query).then((res) => {
        console.log(res.data.data);
        this.stores.mCheck.data = res.data.data;
        this.stores.mCheck.query.totalCount = res.data.totalCount;
      });
    },
    loadmCheckInfoList() {
      GetInfoList(this.stores.mCheckInfo.query).then((res) => {
        console.log(res);
        this.stores.mCheckInfo.data = res.data.data;
        this.stores.mCheckInfo.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    handleSelect2(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleSelectionChange2(selection) {
      this.formInfo.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.mCheck.query.currentPage = page;
      this.loadDispatchList();
    },
    handlePageChanged2(page) {
      this.stores.mCheckInfo.query.currentPage = page;
      this.loadmCheckInfoList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.mCheck.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    handlePageSizeChanged2(pageSize) {
      this.stores.mCheckInfo.query.pageSize = pageSize;
      this.loadmCheckInfoList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    rowClsRender2(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    handleRefresh2() {
      this.loadmCheckInfoList();
    },
    //清空
    handleResetFormDispatch() {
      // this.$refs["formdispatch"].resetFields();
      this.$refs["formdispatch"].resetFields();
    },
    handleResetFormDispatch2() {
      // this.$refs["formdispatch"].resetFields();
      this.$refs["form2"].resetFields();
    },
    //清空
    handleResetFormDispatch22() {
      // this.$refs["formdispatch"].resetFields();
      this.$refs["formdispatch22"].resetFields();
    },
    //右边删除（单个删除）
    handleDelete(row) {
      this.doDelete(row.id);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteDepartment(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    handleBatchCommand2(command) {
      if (!this.selectedRowsId2 || this.selectedRowsId2.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand2(command);
        },
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    doBatchCommand2(command) {
      batchCommand2({
        command: command,
        ids: this.selectedRowsId2.join(","),
        oid: this.o_inspection_id,
        iid:this.inspection_id,
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadmCheckInfoList();
          this.loadDispatchList();
          this.formInfo.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },

    xz(e) {
      this.stores.mCheck.query.kw = e;
      this.loadDispatchList();
    },
    //添加按钮
    handleShowCreateWindow() {
      this.hanldeTogetDepartments();
      this.formModel2.mode = "create";
      this.handleResetFormDispatch(); //清空表单
      this.formModel.opened = true;
    },
    handleShowCreateWindow2() {
      this.formModel2.fields.imgUrl="";
      this.defaultList=[];
      this.uploadList=[];
      this.hanldeTogetTypeList();
      this.handleTogetUserList();
      this.formModel2.mode = "create";
      this.handleResetFormDispatch2(); //清空表单
      this.formModel2.opened = true;
    },
    //右边编辑
    handleEdit(row) {
      this.hanldeTogetDepartments();
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(row.id);
    },
    async handleEdit2(row) {
      console.log(row);
      if (this.formModel2.fields.imgUrl != null) {
        this.formModel.dFileName = "";
        this.$refs.upload.fileList = [
          { name: "", status: "finished", showProgress: false }
        ];
      }
      this.hanldeTogetTypeList();
      await this.handleTogetUserList();

      this.formModel2.mode = "edit";
      this.formModel2.opened = true;
      this.formModel2.fields.imgUrl="";
      this.defaultList=[];
      this.uploadList=[];
      this.handleResetFormDispatch2(); //清空表单
      this.doLoadData2(row.id);
    },
    //右边详情
    handleDetail(row) {
      this.formModel1.opened = true;
      this.handleResetFormDispatch22(); //清空表单
      this.doLoadData(row.id);
    },
    async handleDetail2(row) {
      console.log(row);
       this.formModel2.fields.imgUrl="";
      this.defaultList=[];
      this.uploadList=[];
      this.hanldeTogetTypeList();
      await this.handleTogetUserList();

      this.formModel2.mode = "show";
      this.formModel2.opened = true;
      this.handleResetFormDispatch2(); //清空表单
      this.doLoadData2(row.id);
    },
    //查询当前行信息
    doLoadData(id) {
      Load(id).then((res) => {
        if (res.data.code === 200) {
          console.log(res.data.data);
          let data = res.data.data;
          this.formModel.fields = data;
          this.formModel1.fields = data;
        }
      });
    },
    doLoadData2(id) {
      LoadInfo(id).then((res) => {
        if (res.data.code === 200) {
          console.log(res.data.data);
          let data = res.data.data;
          this.formModel2.fields = data;
          if (res.data.data.imgUrl != null) {
            let list = res.data.data.imgUrl.split(",");
            for (let i = 0; i < list.length; i++) {
              this.uploadList.push({
                url:
                  list[i],
                status: "finished",
                name: list[i],
                fileName: list[i]
              });
            }
          }
        }
      });
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    validateUserForm2() {
      let _valid = false;
      this.$refs["form2"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    //保存按钮
    handleSubmitConsumable() {
      let valid = this.validateUserForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.docreateDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditDispatch();
        }
      }
    },
    handleSubmitConsumable2() {
      let valid = this.validateUserForm2();
      if (valid) {
        if (this.formModel2.mode === "create") {
          this.docreateDispatch2();
        }
        if (this.formModel2.mode === "edit") {
          this.doEditDispatch2();
        }
      }
    },
    //添加（保存）
    docreateDispatch() {
      console.log(this.formModel.fields);
      let data = {
        organizationId: 0,
        organizationName: "",
        inspector: this.formModel.fields.inspector,
        shouldCount: this.formModel.fields.shouldCount,
        departmentId: this.formModel.fields.departmentId,
        id: 0,
        type: 0,
        supplier: "",
        note: "",
        createdAt: null,
        createdUser: 0,
      };
      Create(data).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    docreateDispatch2() {
      console.log(this.formModel2.fields);
      this.formModel2.fields.ownerInspectionId = this.o_inspection_id;
      this.formModel2.fields.inspectionId = this.inspection_id;
      this.formModel2.fields.peopleStatus = this.checkTypes.join(",");
      console.log(this.checkTypes);
      let tnames = [];
      for (let i = 0; i < this.checkTypes.length; i++) {
        let name = this.typelist.find((x) => x.typeId == this.checkTypes[i])
          .keyValue;
        console.log(name);
        tnames.push(name);
      }
      this.formModel2.fields.peopleStatusName = tnames.join(",");
      console.log(this.formModel2.fields.peopleStatusName);
      console.log(this.formModel2.fields);
      //return;
      CreateInfo(this.formModel2.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel2.opened = false; //关闭表单
          this.loadmCheckInfoList();
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch() {
      //this.datadeal();
      console.log(this.formModel.fields);
      let data = {
        organizationId: 0,
        organizationName: "",
        inspector: this.formModel.fields.inspector,
        shouldCount: this.formModel.fields.shouldCount,
        departmentId: this.formModel.fields.departmentId,
        id: this.formModel.fields.id,
        type: 0,
        supplier: "",
        note: "",
        createdAt: null,
        createdUser: 0,
      };
      return;
      EditInfo(data).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadmCheckInfoList();
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditDispatch2() {
      //this.datadeal();
      console.log(this.formModel2.fields);
      this.formModel2.fields.peopleStatus = this.checkTypes.join(",");
      console.log(this.checkTypes);
      let tnames = [];
      for (let i = 0; i < this.checkTypes.length; i++) {
        let name = this.typelist.find((x) => x.typeId == this.checkTypes[i])
          .keyValue;
        console.log(name);
        tnames.push(name);
      }
      this.formModel2.fields.peopleStatusName = tnames.join(",");
      console.log(this.formModel2.fields.peopleStatusName);
      console.log(this.formModel2.fields);
      //return;
      EditInfo(this.formModel2.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel2.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //导入相关操作
    handleImportCuisine() {
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      this.$refs.inputer.value = "";
      this.isexitexcel = false;
      this.formimport.opened = true;
    },
    //下载模板
    handleimportmodel() {
      console.log(this.url);
      window.location.href =
        this.url + "UploadFiles/ImportmCheckModel/餐饮信息导入模板.xlsx";
    },
    //导入
    importfxx(e) {
      let inputDOM = this.$refs.inputer;
      //声明一个FormDate对象
      let formData = new FormData();
      let file = inputDOM.files[0];
      let AllUpExt = ".xls|.xlsx|";
      let extName = file.name
        .substring(file.name.lastIndexOf("."))
        .toLowerCase();
      if (AllUpExt.indexOf(extName + "|") == "-1") {
        this.$refs.inputer.value = "";
        this.$Message.warning("文件格式不正确!");
      } else {
        if (file != null && file != undefined) {
          this.isexitexcel = true;
          formData.append("excelFile", file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
    },
    async handleimport() {
      this.importdisable = true;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      if (this.isexitexcel) {
        await GetImport(this.exceldata).then((res) => {
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadDispatchList();
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$refs.inputer.value = "";
          this.exceldata = new FormData();
          this.isexitexcel = false;
        });
      } else {
        this.$Message.warning("请选择文件");
      }
      this.importdisable = false;
    },
    //导出
    handleExportCuisine(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doCuisineExport(command);
        },
      });
    },
    doCuisineExport(command) {
      console.log(this.selectedRowsId.join(","));
      GetExport(this.selectedRowsId.join(",")).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.selection = [];
          console.log(res);
          window.location.href = config.baseUrl.dev + res.data.data;
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //一键导出
    handleExportCuisineyj(command) {
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doCuisineExport(command);
        },
      });
    },
    handleShowInfoList(row) {
      console.log(row);
      this.o_inspection_id = 0;
      this.inspection_id = 0;
      if (row.inspectionId == 0) {
        this.o_inspection_id = row.id;
      } else {
        this.inspection_id = row.inspectionId;
      }

      this.formInfo.opened = true;
      this.stores.mCheckInfo.query.inspectionid = row.inspectionId;
      this.stores.mCheckInfo.query.id = row.id;
      this.loadmCheckInfoList();
    },
    hanldeTogetDepartments() {
      GetDepartments().then((res) => {
        console.log(2222);
        console.log(res);
        this.departments = res.data.data;
      });
    },
    hanldeTogetTypeList() {
      TypeList().then((res) => {
        console.log("TTTTTTTT");
        console.log(res);
        this.typelist = res.data.data;
      });
    },
    async handleTogetUserList() {
      await UsersList().then((res) => {
        console.log("uuuuUUUU");
        console.log(res);
        this.userlist = res.data.data;
      });
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
      console.log(this.imgName);
    },
    handleRemove(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      // deletetoFile({ path: file }).then(res => {
        // console.log(res);
        // if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel2.fields.imgUrl = this.uploadList
            .map(x => x.fileName)
            .join(",");
        // } else {
        //   this.uploadList = this.uploadList.filter(x => x.name != file);
        //   this.formModel.fields.imgUrl = this.uploadList
        //     .map(x => x.fileName)
        //     .join(",");
        // }
      // });
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg"
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M."
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    toUpResult() {
      console.log(1111);
      console.log(this.$refs.upload.fileList);
      //console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
        // this.$refs.upload.clearFiles();
        // this.$refs.upload.push({})
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        console.log(this.formModel2.fields.imgUrl);
        if (this.formModel2.fields.imgUrl != null) {
          if (this.formModel2.fields.imgUrl.length > 0) {
            this.formModel2.fields.imgUrl += ",";
          }
          this.formModel2.fields.imgUrl += response.data;
        } else {
          this.formModel2.fields.imgUrl = response.data;
        }
        await this.uploadList.push({
          url: response.data.replace("\\", "/"),
          status: "finished",
          name: response.data,
          fileName: response.data
        });
        // console.log(
        //   (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        // );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
      } else {
        this.$Message.warning(response.message);
      }
    },
    checkShow() {
      return this.formModel2.mode === "show";
    },
  },
  mounted() {
    this.loadDispatchList(); //页面加载
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/UpImage/ToUPPhoto/UpPhoto";
  },
};
</script>
<style>
.file1:hover + .fileimg1:hover {
  transform: scale(1.01, 1.01);
  box-shadow: 0 0 3px #1783ba;
}
.fileimg1 {
  width: 300px;
  height: 169px;
  float: left;
  z-index: 2;
}

.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>